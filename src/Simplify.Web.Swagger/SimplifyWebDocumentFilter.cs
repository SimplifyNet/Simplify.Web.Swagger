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
		private static SimplifyWebSwaggerArgs? _args;
		
		/// <summary>
		/// Initializes an instance of <see cref="SimplifyWebDocumentFilter"/>.
		/// </summary>
		/// <param name="args">The registration args</param>
		public SimplifyWebDocumentFilter(SimplifyWebSwaggerArgs? args) => _args = args;

		/// <summary>
		/// Applies current filter
		/// </summary>
		/// <param name="openApiDocument">The Open API document</param>
		/// <param name="context">The context</param>
		public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
		{
			foreach (var item in ControllerActionsFactory.CreateControllerActionsFromControllersMetaData(context)
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

			operation.Parameters = CreateParameters(item.ParsedPath);
			operation.RequestBody = item.RequestBody;

			if (_args != null)
				foreach (var parameter in _args.Parameters)
					operation.Parameters.Add(parameter);

			return operation;
		}

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