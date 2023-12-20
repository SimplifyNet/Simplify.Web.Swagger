using System;

namespace Simplify.Web.Swagger;

/// <summary>
/// A filter that specifies the request body received by the controller.
/// </summary>
public class RequestBodyAttribute : Attribute
{
	/// <summary>
	/// Initializes an instance of <see cref="RequestBodyAttribute"/>.
	/// </summary>
	/// <param name="model">The request body model type.</param>
	public RequestBodyAttribute(Type model) => Model = model ?? throw new ArgumentNullException(nameof(model));

	/// <summary>
	/// Request body model type
	/// </summary>
	public Type Model { get; private set; }
}