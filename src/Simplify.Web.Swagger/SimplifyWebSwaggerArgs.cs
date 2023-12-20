using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Simplify.Web.Swagger;

/// <summary>
/// SimplifyWebSwagger registration args.
/// </summary>
public class SimplifyWebSwaggerArgs
{
	/// <summary>
	/// Open Api Parameters
	/// </summary>
	public List<OpenApiParameter> Parameters { get; } = new ();
}