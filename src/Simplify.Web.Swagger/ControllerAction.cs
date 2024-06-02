using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Simplify.Web.Controllers.Meta.Routing;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Represent controller action
	/// </summary>
	public class ControllerAction
	{
		private ControllerActionNames? _names;
		private IControllerRoute? _controllerRoute;

		/// <summary>
		/// Request body
		/// </summary>
		public OpenApiRequestBody RequestBody { get; set; } = new();

		/// <summary>
		/// Controller responses
		/// </summary>
		public IDictionary<int, OpenApiResponse> Responses { get; set; } = new Dictionary<int, OpenApiResponse>();

		/// <summary>
		/// Operation type
		/// </summary>
		public OperationType Type { get; set; }

		/// <summary>
		/// Controller path
		/// </summary>
#if NETSTANDARD2_0
		public string Path => ControllerRoute.Path.StartsWith("/") ? ControllerRoute.Path : "/" + ControllerRoute.Path;
#else
		public string Path => ControllerRoute.Path.StartsWith('/') ? ControllerRoute.Path : "/" + ControllerRoute.Path;
#endif

		/// <summary>
		/// Gets or sets the controller route.
		/// </summary>
		/// <value>
		/// The controller route.
		/// </value>
		/// <exception cref="InvalidOperationException">ControllerRoute is null</exception>
		public IControllerRoute ControllerRoute
		{
			get => _controllerRoute ?? throw new InvalidOperationException("ControllerRoute is null");
			set => _controllerRoute = value;
		}

		/// <summary>
		/// Controller names
		/// </summary>
		public ControllerActionNames Names
		{
			get => _names ?? throw new InvalidOperationException("Names is null");
			set => _names = value;
		}

		/// <summary>
		/// Gets or sets the value indicating whether controller requires user authorization.
		/// </summary>
		public bool IsAuthorizationRequired { get; set; }
	}
}