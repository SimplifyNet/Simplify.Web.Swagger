using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Simplify.Web.Swagger;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides IServiceCollection for Simplify.Web.Swagger
/// </summary>
public static class SimplifyWebSwaggerServiceCollectionExtensions
{
	/// <summary>
	/// Add the Simplify.Web controllers as Swagger source
	/// </summary>
	/// <param name="services">Services collection</param>
	public static IServiceCollection AddSimplifyWebSwagger(this IServiceCollection services) => services
		.AddSingleton<IApiDescriptionGroupCollectionProvider, SimplifyApiDescriptionGroupCollectionProvider>();
}
