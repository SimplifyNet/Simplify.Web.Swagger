using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Simplify.Web.Meta;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Provides Swagger DocumentFilter for Simplify.Web framework
	/// </summary>
	public class SimplifyWebDocumentFilter : IDocumentFilter
	{
		// private const string VersionEndPoint = "/version";

		private static IList<string> RemovePrefixes = new List<string>
			{
				"Controllers.",
				"Api.v1."
			};

		/// <summary>
		/// Applies current filter
		/// </summary>
		/// <param name="openApiDocument">The Open API document</param>
		/// <param name="context">The context</param>

		public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
		{
			foreach (var item in CreatePathItemsFromControllersMetaData())
				openApiDocument?.Paths.Add(item.Key, item.Value);
		}

		private static IDictionary<string, OpenApiPathItem> CreatePathItemsFromControllersMetaData() =>
			ControllersMetaStore.Current.ControllersMetaData
			.Where(x => x.ExecParameters != null)
			.SelectMany(CreatePathItems)
			.ToDictionary(x => x.Key, x => x.Value);

		private static IEnumerable<KeyValuePair<string, OpenApiPathItem>> CreatePathItems(IControllerMetaData item)
		{
			var items = new List<KeyValuePair<string, OpenApiPathItem>>();
			var routes = item.ExecParameters!.Routes;
			var needToAddPostfix = routes.ContainsDuplicates();

			items.AddRange(routes.Select(x => new KeyValuePair<string, OpenApiPathItem>(
				FormatControllerName(x.Value, x.Key, needToAddPostfix), CreateOpenApiPathItems(x.Key, x.Value, item))));

			return items;
		}

		private static OpenApiPathItem CreateOpenApiPathItems(HttpMethod method, string route, IControllerMetaData item)
		{
			var pathItem = new OpenApiPathItem();

			pathItem.AddOperation(HttpMethodToOperationType(method), CreateOperation(item));

			return pathItem;
		}

		private static OpenApiOperation CreateOperation(IControllerMetaData item)
		{
			// TODO

			var operation = new OpenApiOperation
			{
			};

			operation.Tags.Add(new OpenApiTag
			{
				Name = FormatOperationName(item)
			});

			return operation;
		}

		private static OperationType HttpMethodToOperationType(HttpMethod method) =>
			method switch
			{
				HttpMethod.Get => OperationType.Get,
				HttpMethod.Post => OperationType.Post,
				HttpMethod.Put => OperationType.Put,
				HttpMethod.Patch => OperationType.Patch,
				HttpMethod.Delete => OperationType.Delete,
				HttpMethod.Options => OperationType.Options,
				_ => OperationType.Get
			};

		// TODO
		private static KeyValuePair<string, OpenApiResponse> CreateResponse()
		{
			var response = new OpenApiResponse
			{
			};

			return new KeyValuePair<string, OpenApiResponse>("", response);
		}

		private static string FormatControllerName(string route, HttpMethod method, bool needToAddPostfix) =>
			needToAddPostfix ? $"{route} ({method})" : route;

		private static string? FormatOperationName(IControllerMetaData item) =>
			item.ControllerType.FullName != null ? FormatOperationName(item.ControllerType.FullName) : null;

		private static string FormatOperationName(string str)
		{
			foreach (var prefix in RemovePrefixes)
			{
				var prefixIndex = str.IndexOf(prefix);

				if (prefixIndex == -1)
					continue;

				str = str.Substring(prefixIndex + prefix.Length);
			}

			str = str.Replace(".", "/");

			if (str.EndsWith("Controller"))
				str = str.Substring(0, str.LastIndexOf("Controller"));

			return str;
		}
	}
}