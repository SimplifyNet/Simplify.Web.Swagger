using System.Collections.Generic;

namespace Simplify.Web.Swagger;

/// <summary>
/// Configuration for the Accept-Language header parameter added to all operations.
/// </summary>
public class AcceptLanguageHeaderArgs
{
	/// <summary>
	/// Initializes a new instance with the specified supported languages.
	/// The first entry is used as the default value.
	/// </summary>
	/// <param name="languages">Supported language codes, e.g. "en-US", "ru-RU".</param>
	public AcceptLanguageHeaderArgs(params string[] languages)
	{
		Languages = languages;
		Default = languages.Length > 0 ? languages[0] : string.Empty;
	}

	/// <summary>
	/// The list of supported language codes.
	/// </summary>
	public IReadOnlyList<string> Languages { get; }

	/// <summary>
	/// The default language code (defaults to the first entry in <see cref="Languages"/>).
	/// </summary>
	public string Default { get; set; }
}