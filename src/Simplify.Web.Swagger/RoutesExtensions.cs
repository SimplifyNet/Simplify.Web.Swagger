using System.Collections.Generic;
using System.Linq;
using Simplify.Web.Http;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the routes extension methods.
/// </summary>
public static class RoutesExtensions
{
	/// <summary>
	/// Check if the controller have duplicate paths
	/// </summary>
	/// <param name="items">The items.</param>
	/// <returns>
	///   <c>true</c> if the specified items contains duplicates; otherwise, <c>false</c>.
	/// </returns>
	public static bool ContainsDuplicates(this IDictionary<HttpMethod, string> items) =>
		items.GroupBy(x => x.Value).Any(g => g.Count() > 1);
}