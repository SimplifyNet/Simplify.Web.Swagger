using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the Swagger extensions for Simplify.Web.
/// </summary>
public static class SimplifyWebSwaggerServiceCollectionExtensions
{
	/// <summary>
	/// Registers Simplify.Web Swagger services on the service collection.
	/// Defaults schema property naming to PascalCase to match Simplify.Web default serialization.
	/// Must be called before <c>AddSwaggerGen</c>.
	/// </summary>
	/// <param name="services">The service collection.</param>
	public static IServiceCollection AddSimplifyWebSwaggerServices(this IServiceCollection services)
	{
		// Swashbuckle resolves JsonSerializerOptions from Mvc.JsonOptions or HttpJsonOptions, both of which
		// default to CamelCase via JsonSerializerDefaults.Web. Simplify.Web serializes independently using
		// PascalCase by default. Register a custom resolver with PropertyNamingPolicy = null (= PascalCase)
		// before AddSwaggerGen so that Swashbuckle's TryAddSingleton<ISerializerDataContractResolver> is skipped.
		services.AddSingleton<ISerializerDataContractResolver>(_ =>
		new JsonSerializerDataContractResolver(new JsonSerializerOptions { PropertyNamingPolicy = null }));

		return services;
	}

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

		options.SchemaFilter<EnumNamesSchemaFilter>();
	}
}