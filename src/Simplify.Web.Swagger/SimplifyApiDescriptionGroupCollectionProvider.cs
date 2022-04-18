using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Simplify.Web.Meta;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides IApiDescriptionGroupCollectionProvider implementation for Simplify.Web framework
/// </summary>
public class SimplifyApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
{
	private ApiDescriptionGroupCollection? _apiDescriptionGroups;

	/// <summary>
	/// Gets the API description groups
	/// </summary>
	public ApiDescriptionGroupCollection ApiDescriptionGroups => _apiDescriptionGroups ??= LoadApiDescriptionGroup();

	private ApiDescriptionGroupCollection LoadApiDescriptionGroup() => new ApiDescriptionGroupCollection(LoadApiDescriptionGroups(), 1);

	private IReadOnlyList<ApiDescriptionGroup> LoadApiDescriptionGroups() =>
		new List<ApiDescriptionGroup> { new ApiDescriptionGroup("Simplify.Web Controllers", LoadApiDescriptions()) };

	private IReadOnlyList<ApiDescription> LoadApiDescriptions() => ControllersMetaStore.Current.ControllersMetaData.Select(CreateApiDescription).ToList();

	private ApiDescription CreateApiDescription(IControllerMetaData item)
	{
		var desc = new ApiDescription();

		return desc;
	}
}
