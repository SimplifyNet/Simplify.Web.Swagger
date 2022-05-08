using System;
using System.Collections.Generic;
using System.Linq;
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
				Names = CreateNames(item.ControllerType)
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
	}
}