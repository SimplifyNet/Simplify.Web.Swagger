using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the SimplifyWeb Swagger registration args.
/// </summary>
public class SimplifyWebSwaggerArgs
{
	/// <summary>
	/// Open Api Parameters
	/// </summary>
	public IList<OpenApiParameter> Parameters { get; } = [];
}