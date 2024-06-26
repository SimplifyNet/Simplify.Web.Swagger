using System;
using System.Collections.Generic;

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the filter that specifies the type of the value and status code returned by the controller.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ProducesResponseAttribute : Attribute
{
	/// <summary>
	/// Initializes an instance of <see cref="ProducesResponseAttribute" />.
	/// </summary>
	/// <param name="statusCode">The HTTP response status code.</param>
	/// <param name="contentType">The content type associated with the response.</param>
	/// <param name="additionalContentTypes">Additional content types supported by the response.</param>
	public ProducesResponseAttribute(int statusCode, string? contentType, params string[] additionalContentTypes) :
		this(statusCode, null, contentType, additionalContentTypes)
	{
	}

	/// <summary>
	/// Initializes an instance of <see cref="ProducesResponseAttribute"/>.
	/// </summary>
	/// <param name="statusCode">The HTTP response status code.</param>
	/// <param name="type">The <see cref="Type"/> of object that is going to be written in the response.</param>
	/// <param name="contentType">The content type associated with the response.</param>
	/// <param name="additionalContentTypes">Additional content types supported by the response.</param>
	public ProducesResponseAttribute(int statusCode, Type? type = null, string? contentType = null, params string[] additionalContentTypes)
	{
		StatusCode = statusCode;
		Type = type;

		if (!string.IsNullOrEmpty(contentType))
			ContentTypes.Add(contentType!);

		foreach (var t in additionalContentTypes)
		{
			if (string.IsNullOrEmpty(t))
				continue;

			ContentTypes.Add(t);
		}
	}

	/// <summary>
	/// Gets the HTTP status code of the response.
	/// </summary>
	/// <value>
	/// The status code.
	/// </value>
	public int StatusCode { get; }

	/// <summary>
	/// Gets the type of the value returned by a controller.
	/// </summary>
	/// <value>
	/// The type.
	/// </value>
	public Type? Type { get; }

	/// <summary>
	/// Gets the HTTP content types of the response
	/// </summary>
	/// <value>
	/// The content types.
	/// </value>
	public IList<string> ContentTypes { get; } = [];
}