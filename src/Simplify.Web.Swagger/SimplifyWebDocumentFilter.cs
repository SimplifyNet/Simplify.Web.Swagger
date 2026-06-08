using System;
using System.Collections.Generic;
using System.Linq;
using Simplify.Web.Controllers.Meta.Routing;
using Swashbuckle.AspNetCore.SwaggerGen;
#if NET10_0
using Microsoft.OpenApi;
using JsonNode = System.Text.Json.Nodes.JsonNode;
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
					CreatePathItem(x, swaggerDoc, context)
				))
		)
			swaggerDoc.Paths.Add(item.Key, item.Value);

		PopulateDocumentTags(swaggerDoc, controllerActions);
	}

#if NET10_0
	private static IList<IOpenApiParameter> CreateParameters(ControllerAction item, DocumentFilterContext context) =>
		item.ControllerRoute.Items
			.OfType<PathParameter>()
			.Select(x => (IOpenApiParameter)CreatePathParameter(x, item, context))
			.ToList();
#else
	private static IList<OpenApiParameter> CreateParameters(ControllerAction item, DocumentFilterContext context) =>
		item.ControllerRoute.Items
			.OfType<PathParameter>()
			.Select(x => CreatePathParameter(x, item, context))
			.ToList();
#endif

	private static OpenApiParameter CreatePathParameter(PathParameter pathParam, ControllerAction item, DocumentFilterContext context)
	{
		var param = new OpenApiParameter
		{
			Name = pathParam.Name,
			In = ParameterLocation.Path,
			Required = true,
			AllowEmptyValue = false,
		};

		if (item.RouteParameterTypes.TryGetValue(pathParam.Name, out var type))
			param.Schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

		return param;
	}

	private OpenApiPathItem CreatePathItem(
		IEnumerable<ControllerAction> actions,
		OpenApiDocument swaggerDoc,
		DocumentFilterContext context
	)
	{
		var pathItem = new OpenApiPathItem();

		foreach (var item in actions)
			pathItem.AddOperation(item.Type, CreateOperation(item, swaggerDoc, context));

		return pathItem;
	}

	private OpenApiOperation CreateOperation(ControllerAction item, OpenApiDocument swaggerDoc, DocumentFilterContext context)
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

		operation.Parameters = CreateParameters(item, context);

		if (item.RequestBody.Content is { Count: > 0 })
			operation.RequestBody = item.RequestBody;

		if (item.IsAuthorizationRequired)
		{
			var schemeNames = ResolveSecuritySchemeNames(swaggerDoc);

#if NET10_0
			if (schemeNames.Count > 0)
				operation.Security = schemeNames
					.Select(name => new OpenApiSecurityRequirement
					{
						[new OpenApiSecuritySchemeReference(name, swaggerDoc)] = [],
					})
					.ToList();
#else
			if (schemeNames.Count > 0)
				operation.Security = schemeNames
					.Select(name => new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = name,
								},
							},
							new List<string>()
						},
					})
					.ToList();
#endif
		}

		if (_args == null)
			return operation;

		foreach (var parameter in _args.Parameters)
			operation.Parameters.Add(parameter);

		if (_args.AcceptLanguageHeader is { } acceptLang)
			operation.Parameters.Add(CreateAcceptLanguageParameter(acceptLang));

		return operation;
	}

	private IReadOnlyList<string> ResolveSecuritySchemeNames(OpenApiDocument swaggerDoc)
	{
		if (_args?.SecuritySchemeName is { } explicitName)
			return [explicitName];

#if NET10_0
		return swaggerDoc.Components?.SecuritySchemes?.Keys?.ToList() ?? [];
#else
		return swaggerDoc.Components.SecuritySchemes.Keys.ToList();
#endif
	}

	private static OpenApiParameter CreateAcceptLanguageParameter(AcceptLanguageHeaderArgs args)
	{
		var param = new OpenApiParameter
		{
			Name = "Accept-Language",
			In = ParameterLocation.Header,
			Description = "Language preference for the response.",
			Required = true,
			AllowEmptyValue = true,
		};

#if NET10_0
		param.Example = args.Default;
		param.Schema = new OpenApiSchema
		{
			Default = args.Default,
			Enum = args.Languages.Select(l => (JsonNode)l).ToList()
		};
#else
		param.Example = new Microsoft.OpenApi.Any.OpenApiString(args.Default);
		param.Schema = new OpenApiSchema
		{
			Default = new Microsoft.OpenApi.Any.OpenApiString(args.Default),
			Enum = args.Languages.Select(l => (Microsoft.OpenApi.Any.IOpenApiAny)new Microsoft.OpenApi.Any.OpenApiString(l)).ToList()
		};
#endif

		return param;
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