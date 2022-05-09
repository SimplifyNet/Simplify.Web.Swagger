using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Simplify.Web.Meta;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Provides ControllerAction factory
	/// </summary>
	public class ControllerActionsFactory
	{
		/// <summary>
		/// Provides controller prefixes to remove
		/// </summary>
		public static IList<string> RemovePrefixes = new List<string>
			{
				"Controllers.",
				"Api.v1."
			};

		/// <summary>
		/// Creates controller actions from Simplify.Web controller meta data
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<ControllerAction> CreateControllerActionsFromControllersMetaData() =>
			ControllersMetaStore.Current.ControllersMetaData
				.Where(x => x.ExecParameters != null)
				.SelectMany(CreateControllerActions);

		private static IEnumerable<ControllerAction> CreateControllerActions(IControllerMetaData item) =>
			item.ExecParameters!
				.Routes
				.Select(x => CreateControllerAction(x.Key, x.Value, item));

		private static ControllerAction CreateControllerAction(HttpMethod method, string route, IControllerMetaData item) =>
			new ControllerAction
			{
				Type = HttpMethodToOperationType(method),
				Path = route.StartsWith("/") ? route : "/" + route,
				Names = CreateNames(item.ControllerType),
				Responses = CreateResponses(item.ControllerType)
			};

		private static ControllerActionNames CreateNames(Type controllerType) =>
			CreateNames(controllerType.FullName ?? throw new InvalidOperationException("controllerType.FullName is null"));

		private static ControllerActionNames CreateNames(string name)
		{
			var src = FormatNameSource(name);

			var index = src.LastIndexOf("/");

			if (index == -1)
				return new ControllerActionNames(src, src);

			return new ControllerActionNames(src, src.Substring(0, index), src.Substring(index + 1));
		}

		private static string FormatNameSource(string str)
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

		private static IDictionary<int, OpenApiResponse> CreateResponses(Type controllerType)
		{
			var items = new Dictionary<int, OpenApiResponse>();

			var attributes = controllerType.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), false);

			foreach (ProducesResponseTypeAttribute item in attributes)
				items.Add(item.StatusCode, CreateResponse(item));

			return items;
		}

		private static OpenApiResponse CreateResponse(ProducesResponseTypeAttribute producesResponse)
		{
			var response = new OpenApiResponse();

			response.Description = ResponseDescriptionMap
				.FirstOrDefault((entry) => Regex.IsMatch(producesResponse.StatusCode.ToString(), entry.Key))
				.Value;

			return response;
		}

		private static readonly IReadOnlyCollection<KeyValuePair<string, string>> ResponseDescriptionMap = new[]
	  {
		   new KeyValuePair<string, string>("1\\d{2}", "Information"),

			new KeyValuePair<string, string>("201", "Created"),
			new KeyValuePair<string, string>("202", "Accepted"),
			new KeyValuePair<string, string>("204", "No Content"),
			new KeyValuePair<string, string>("2\\d{2}", "Success"),

			new KeyValuePair<string, string>("304", "Not Modified"),
			new KeyValuePair<string, string>("3\\d{2}", "Redirect"),

			new KeyValuePair<string, string>("400", "Bad Request"),
			new KeyValuePair<string, string>("401", "Unauthorized"),
			new KeyValuePair<string, string>("403", "Forbidden"),
			new KeyValuePair<string, string>("404", "Not Found"),
			new KeyValuePair<string, string>("405", "Method Not Allowed"),
			new KeyValuePair<string, string>("406", "Not Acceptable"),
			new KeyValuePair<string, string>("408", "Request Timeout"),
			new KeyValuePair<string, string>("409", "Conflict"),
			new KeyValuePair<string, string>("429", "Too Many Requests"),
			new KeyValuePair<string, string>("4\\d{2}", "Client Error"),

			new KeyValuePair<string, string>("5\\d{2}", "Server Error"),
			new KeyValuePair<string, string>("default", "Error")
		};
	}
}