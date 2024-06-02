using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the Swagger extensions for Simplify.Web.
/// </summary>
public static class SimplifyWebSwaggerServiceCollectionExtensions
{
	/// <summary>
	/// Add Simplify.Web controllers to Swagger documentation generation
	/// </summary>
	/// <param name="options">The options.</param>
	/// <param name="args">The arguments.</param>
	public static void AddSimplifyWebSwagger(this SwaggerGenOptions options, SimplifyWebSwaggerArgs? args = null)
	{
		if (args is null)
			options.DocumentFilter<SimplifyWebDocumentFilter>();
		else
			options.DocumentFilter<SimplifyWebDocumentFilter>(args);
	}
}