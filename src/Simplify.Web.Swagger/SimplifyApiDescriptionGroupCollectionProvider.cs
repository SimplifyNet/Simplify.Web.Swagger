using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides IApiDescriptionGroupCollectionProvider implementation for Simplify.Web framework
/// </summary>
public class SimplifyApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
{
	/// <summary>
	/// Gets the API description groups
	/// </summary>
	public ApiDescriptionGroupCollection ApiDescriptionGroups => throw new global::System.NotImplementedException();
}
