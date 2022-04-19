using System;
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

	private IReadOnlyList<ApiDescription> LoadApiDescriptions() =>
		ControllersMetaStore.Current.ControllersMetaData
		.Where(x => x.ExecParameters != null)
		.SelectMany(CreateApiDescriptions)
		.ToList();

	private IList<ApiDescription> CreateApiDescriptions(IControllerMetaData item)
	{
		if (item.ExecParameters == null)
			throw new InvalidOperationException();

		return item.ExecParameters.Routes.Select(x => CreateApiDescription(x.Key, x.Value, item)).ToList();
	}

	private ApiDescription CreateApiDescription(HttpMethod method, string route, IControllerMetaData item)
	{
		var desc = new ApiDescription();

		desc.HttpMethod = GetHttpMethod(method);

		return desc;
	}

	private string GetHttpMethod(HttpMethod method) => method switch
	{
		HttpMethod.Get => "GET",
		HttpMethod.Post => "POST",
		HttpMethod.Put => "PUT",
		HttpMethod.Patch => "PATCH",
		HttpMethod.Delete => "DELETE",
		HttpMethod.Options => "OPTIONS",
		_ => ""
	};
}
