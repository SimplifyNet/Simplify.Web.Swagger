using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
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

			return operation;
		}
	}
}