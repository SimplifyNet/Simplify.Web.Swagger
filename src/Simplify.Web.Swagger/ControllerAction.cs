using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Simplify.Web.Routing;

namespace Simplify.Web.Swagger
{
	/// <summary>
	/// Represent controller action
	/// </summary>
	public class ControllerAction
	{
		private string? _path;
		private ControllerActionNames? _names;

		/// <summary>
		/// Operation type
		/// </summary>
		public OperationType Type { get; set; }

		/// <summary>
		/// Controller responses
		/// </summary>
		public IDictionary<int, OpenApiResponse> Responses = new Dictionary<int, OpenApiResponse>();

		/// <summary>
		/// Controller path
		/// </summary>
		public string Path
		{
			get => _path ?? throw new InvalidOperationException("Path is null");
			set => _path = value;
		}

		/// <summary>
		/// Controller parsed path
		/// </summary>
		public IControllerPath ParsedPath => new ControllerPathParser().Parse(Path);

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