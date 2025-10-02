using System;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the filter that specifies the request body received by the controller.
/// </summary>
/// <seealso cref="Attribute" />
/// <remarks>
/// Initializes an instance of <see cref="RequestBodyAttribute" />.
/// </remarks>
/// <param name="model">The request body model type.</param>
/// <param name="contentType">The content type associated with the request.</param>
[AttributeUsage(AttributeTargets.Class)]
public class RequestBodyAttribute(Type model, string contentType = "application/json") : Attribute
{
	/// <summary>
	/// Request body model type
	/// </summary>
	/// <value>
	/// The model.
	/// </value>
	public Type Model { get; private set; } = model ?? throw new ArgumentNullException(nameof(model));

	/// <summary>
	/// Gets the type of the content.
	/// </summary>
	/// <value>
	/// The type of the content.
	/// </value>
	public string ContentType { get; private set; } = contentType;
}
