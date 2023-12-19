using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides Swagger extensions for Simplify.Web
/// </summary>
public static class SimplifyWebSwaggerServiceCollectionExtensions
{
	/// <summary>
	/// Add Simplify.Web controllers to Swagger documentation generation
	/// </summary>
	public static void AddSimplifyWebSwagger(this SwaggerGenOptions options, SimplifyWebSwaggerArgs? args = null) => options.DocumentFilter<SimplifyWebDocumentFilter>(args);
}