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

	/// <summary>
	/// When set, adds an <c>Accept-Language</c> header parameter to every operation.
	/// </summary>
	public AcceptLanguageHeaderArgs? AcceptLanguageHeader { get; set; }

	/// <summary>
	/// The security scheme name to apply to authorized operations (e.g. "Bearer").
	/// When <c>null</c> (default), all security schemes registered via <c>AddSecurityDefinition</c>
	/// are applied automatically. Set to an explicit name to restrict to a single scheme.
	/// </summary>
	public string? SecuritySchemeName { get; set; }
}