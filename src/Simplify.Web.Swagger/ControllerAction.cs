using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Simplify.Web.Controllers.Meta.Routing;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Provides the controller action.
	/// </summary>
	public class ControllerAction
	{
		private ControllerActionNames? _names;
		private IControllerRoute? _controllerRoute;

		/// <summary>
		/// Gets or sets the request body.
		/// </summary>
		/// <value>
		/// The request body.
		/// </value>
		public OpenApiRequestBody RequestBody { get; set; } = new();

		/// <summary>
		/// Gets or sets the responses.
		/// </summary>
		/// <value>
		/// The responses.
		/// </value>
		public IDictionary<int, OpenApiResponse> Responses { get; set; } = new Dictionary<int, OpenApiResponse>();

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public OperationType Type { get; set; }

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
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
		/// Gets or sets the names.
		/// </summary>
		/// <value>
		/// The names.
		/// </value>
		/// <exception cref="InvalidOperationException">Names is null</exception>
		public ControllerActionNames Names
		{
			get => _names ?? throw new InvalidOperationException("Names is null");
			set => _names = value;
		}

		/// <summary>
		/// Gets or sets the value indicating whether controller requires user authorization.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is authorization required; otherwise, <c>false</c>.
		/// </value>
		public bool IsAuthorizationRequired { get; set; }
	}
}