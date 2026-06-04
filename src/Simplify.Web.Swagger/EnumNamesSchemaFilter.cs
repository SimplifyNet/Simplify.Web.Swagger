using System;
using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger;

/// <summary>
/// Adds names extension and a human-readable description to enum schemas.
/// </summary>
public class EnumNamesSchemaFilter : ISchemaFilter
{
	/// <inheritdoc />
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (!context.Type.IsEnum)
			return;

		var names = Enum.GetNames(context.Type);
		var values = Enum.GetValues(context.Type).Cast<int>().ToArray();

		var varnames = new OpenApiArray();
		foreach (var name in names)
			varnames.Add(new OpenApiString(name));

		schema.Extensions["names"] = varnames;
	}
}