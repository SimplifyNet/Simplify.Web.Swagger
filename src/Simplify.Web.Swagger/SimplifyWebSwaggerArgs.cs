using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Simplify.Web.Swagger;

/// <summary>
/// SimplifyWebSwagger registration args.
/// </summary>
public class SimplifyWebSwaggerArgs
{
	/// <summary>
	/// Initializes an instance of <see cref="ProducesResponseAttribute"/>.
	/// </summary>
	public SimplifyWebSwaggerArgs() => Parameters = new List<OpenApiParameter>();

	/// <summary>
	/// Open Api Parameters
	/// </summary>
	public IList<OpenApiParameter> Parameters { get; set; }
}