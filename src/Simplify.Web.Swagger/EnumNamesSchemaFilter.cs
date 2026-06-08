using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger;

/// <summary>
/// Adds names extension and a human-readable description to enum schemas.
/// </summary>
public class EnumNamesSchemaFilter : ISchemaFilter
{
	/// <inheritdoc />
	public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
	{
		if (!context.Type.IsEnum)
			return;

		if (schema is not OpenApiSchema concreteSchema)
			return;

		var names = Enum.GetNames(context.Type);
		var values = Enum.GetValues(context.Type).Cast<object>().ToArray();

		var varnames = new JsonArray();
		foreach (var name in names)
			varnames.Add(name);

		concreteSchema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
		concreteSchema.Extensions["x-varnames"] = new JsonNodeExtension(varnames);
		concreteSchema.Description = BuildDescription(names, values);
	}

	private static string BuildDescription(string[] names, object[] values) =>
		string.Join(", ", names.Select((name, i) => $"{Convert.ToInt64(values[i])} = {name}"));
}