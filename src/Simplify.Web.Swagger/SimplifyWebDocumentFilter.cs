using System;
using System.Collections.Generic;
using System.Linq;
using Simplify.Web.Controllers.Meta.Routing;
using Swashbuckle.AspNetCore.SwaggerGen;
#if NET10_0
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the Swagger <see cref="IDocumentFilter" /> implementation for Simplify.Web framework.
/// </summary>
/// <seealso cref="IDocumentFilter" />
public class SimplifyWebDocumentFilter : IDocumentFilter
{
	private readonly SimplifyWebSwaggerArgs? _args;

	/// <summary>
	/// Initializes an instance of <see cref="SimplifyWebDocumentFilter" />.
	/// </summary>
	public SimplifyWebDocumentFilter() { }

	/// <summary>
	/// Initializes an instance of <see cref="SimplifyWebDocumentFilter" />.
	/// </summary>
	/// <param name="args">The registration args</param>
	public SimplifyWebDocumentFilter(SimplifyWebSwaggerArgs? args) => _args = args;

	/// <summary>
	/// Applies current filter
	/// </summary>
	/// <param name="swaggerDoc">The Open API document</param>
	/// <param name="context">The context</param>
	public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
	{
		var controllerActions = ControllerActionsFactory
			.CreateControllerActionsFromControllersMetaData(context)
			.ToList();

		foreach (
			var item in controllerActions
				.GroupBy(x => x.Path)
				.Select(x => new KeyValuePair<string, OpenApiPathItem>(
					x.Key,
					CreatePathItem(x, swaggerDoc)
				))
		)
			swaggerDoc.Paths.Add(item.Key, item.Value);

		PopulateDocumentTags(swaggerDoc, controllerActions);
	}

#if NET10_0
	private static IList<IOpenApiParameter> CreateParameters(IControllerRoute path) =>
		path
			.Items.Where(x => x is PathParameter)
			.Cast<PathParameter>()
			.Select(x =>
				(IOpenApiParameter)
					new OpenApiParameter
					{
						Name = x.Name,
						In = ParameterLocation.Path,
						AllowEmptyValue = false,
					}
			)
			.ToList();
#else
	private static IList<OpenApiParameter> CreateParameters(IControllerRoute path) =>
		path
			.Items.Where(x => x is PathParameter)
			.Cast<PathParameter>()
			.Select(x => new OpenApiParameter
			{
				Name = x.Name,
				In = ParameterLocation.Path,
				AllowEmptyValue = false,
			})
			.ToList();
#endif

	private OpenApiPathItem CreatePathItem(
		IEnumerable<ControllerAction> actions,
		OpenApiDocument swaggerDoc
	)
	{
		var pathItem = new OpenApiPathItem();

		foreach (var item in actions)
			pathItem.AddOperation(item.Type, CreateOperation(item, swaggerDoc));

		return pathItem;
	}

	private OpenApiOperation CreateOperation(ControllerAction item, OpenApiDocument swaggerDoc)
	{
		var operation = new OpenApiOperation();

#if NET10_0
		operation.Tags ??= new HashSet<OpenApiTagReference>();
		operation.Tags.Add(new OpenApiTagReference(item.Names.GroupName));
#else
		operation.Tags.Add(new OpenApiTag { Name = item.Names.GroupName });
#endif

		if (item.Names.Summary != null)
			operation.Summary = item.Names.Summary;

		foreach (var response in item.Responses)
		{
#if NET10_0
			operation.Responses ??= new OpenApiResponses();
#endif
			operation.Responses.Add(response.Key.ToString(), response.Value);
		}

		operation.Parameters = CreateParameters(item.ControllerRoute);

		if (item.RequestBody.Content is { Count: > 0 })
			operation.RequestBody = item.RequestBody;

#if NET10_0
		if (
			item.IsAuthorizationRequired && _args?.SecuritySchemeName is { } securitySchemeNameNet10
		)
			operation.Security =
			[
				new OpenApiSecurityRequirement
				{
					[new OpenApiSecuritySchemeReference(securitySchemeNameNet10, swaggerDoc)] = [],
				},
			];
#else
		if (item.IsAuthorizationRequired && _args?.SecuritySchemeName is { } securitySchemeName)
			operation.Security = new List<OpenApiSecurityRequirement>
			{
				new()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = securitySchemeName,
							},
						},
						new List<string>()
					},
				},
			};
#endif

		if (_args == null)
			return operation;

		foreach (var parameter in _args.Parameters)
			operation.Parameters.Add(parameter);

		return operation;
	}

	private static void PopulateDocumentTags(
		OpenApiDocument swaggerDoc,
		IEnumerable<ControllerAction> actions
	)
	{
#if NET10_0
		var existingNames =
			swaggerDoc.Tags?.Select(t => t.Name).ToHashSet(StringComparer.Ordinal) ?? [];
#else
		var existingNames = swaggerDoc.Tags.Select(t => t.Name).ToHashSet(StringComparer.Ordinal);
#endif

		foreach (
			var name in actions
				.Select(x => x.Names.GroupName)
				.Distinct()
				.Where(n => !existingNames.Contains(n))
		)
		{
#if NET10_0
			swaggerDoc.Tags ??= new HashSet<OpenApiTag>();
#endif
			swaggerDoc.Tags.Add(new OpenApiTag { Name = name });
		}
	}
}
