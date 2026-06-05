using System;
using System.Collections.Generic;
#if NET10_0
using System.Text.Json.Nodes;
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#endif
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Simplify.Web.Swagger;

/// <summary>
/// Adds names extension and a human-readable description to enum schemas.
/// </summary>
public class EnumNamesSchemaFilter : ISchemaFilter
{
	/// <inheritdoc />
#if NET10_0
	public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
	{
		if (!context.Type.IsEnum)
			return;

		if (schema is not OpenApiSchema concreteSchema)
			return;

		var varnames = new JsonArray();
		foreach (var name in Enum.GetNames(context.Type))
			varnames.Add(name);

		concreteSchema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
		concreteSchema.Extensions["names"] = new JsonNodeExtension(varnames);
	}
#else
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (!context.Type.IsEnum)
			return;

		var varnames = new OpenApiArray();
		foreach (var name in Enum.GetNames(context.Type))
			varnames.Add(new OpenApiString(name));

		schema.Extensions["names"] = varnames;
	}
#endif
}
