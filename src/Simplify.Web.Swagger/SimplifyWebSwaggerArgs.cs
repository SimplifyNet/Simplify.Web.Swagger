using System.Collections.Generic;
#if NET10_0
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif

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