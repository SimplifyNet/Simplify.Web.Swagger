using System;

namespace Simplify.Web.Swagger;

/// <summary>
/// A filter that specifies the request body received by the controller.
/// </summary>
public class ProducesRequestBodyAttribute : Attribute
{
	/// <summary>
	/// Initializes an instance of <see cref="ProducesRequestBodyAttribute"/>.
	/// </summary>
	/// <param name="model">The schema model.</param>
	public ProducesRequestBodyAttribute(Type? model) => Model = model;

	/// <summary>
	/// Request body
	/// </summary>
	public Type? Model { get; set; }
}