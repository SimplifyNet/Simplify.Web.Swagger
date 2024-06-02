using System;

namespace Simplify.Web.Swagger;

/// <summary>
/// A filter that specifies the request body received by the controller.
/// </summary>
/// <seealso cref="Attribute" />
/// <remarks>
/// Initializes an instance of <see cref="RequestBodyAttribute" />.
/// </remarks>
/// <param name="model">The request body model type.</param>
[AttributeUsage(AttributeTargets.Class)]
public class RequestBodyAttribute(Type model) : Attribute
{
	/// <summary>
	/// Request body model type
	/// </summary>
	public Type Model { get; private set; } = model ?? throw new ArgumentNullException(nameof(model));
}