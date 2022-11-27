using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Simplify.Web.Routing;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Provides Swagger DocumentFilter for Simplify.Web framework
	/// </summary>
	public class SimplifyWebDocumentFilter : IDocumentFilter
	{
		/// <summary>
		/// Applies current filter
		/// </summary>
		/// <param name="openApiDocument">The Open API document</param>
		/// <param name="context">The context</param>
		public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
		{
			foreach (var item in ControllerActionsFactory.CreateControllerActionsFromControllersMetaData()
				.GroupBy(x => x.Path)
				.Select(x => new KeyValuePair<string, OpenApiPathItem>(x.Key, CreatePathItem(x))))
				openApiDocument?.Paths.Add(item.Key, item.Value);
		}

		private static OpenApiPathItem CreatePathItem(IEnumerable<ControllerAction> actions)
		{
			var pathItem = new OpenApiPathItem();

			foreach (var item in actions)
				pathItem.AddOperation(item.Type, CreateOperation(item));

			return pathItem;
		}

		private static OpenApiOperation CreateOperation(ControllerAction item)
		{
			var operation = new OpenApiOperation();

			operation.Tags.Add(new OpenApiTag
			{
				Name = item.Names.GroupName
			});

			if (item.Names.Summary != null)
				operation.Summary = item.Names.Summary;

			foreach (var response in item.Responses)
				operation.Responses.Add(response.Key.ToString(), response.Value);

			if (item.IsAuthorizationRequired)
				AddSecurity(operation);

			operation.Parameters = CreateParameters(item.ParsedPath);

			return operation;
		}

		private static void AddSecurity(OpenApiOperation operation) =>
			operation.Security.Add(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference {
							Type = ReferenceType.SecurityScheme,
							Id = "bearerAuth"
						}
					}, new List<string>()
				}
			});

		private static IList<OpenApiParameter> CreateParameters(IControllerPath path) =>
			path.Items
				.Where(x => x is PathParameter)
				.Cast<PathParameter>()
				.Select(x => new OpenApiParameter
				{
					Name = x.Name,
					In = ParameterLocation.Path,
					AllowEmptyValue = false
				}).ToList();
	}
}