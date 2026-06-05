using System.Text.Json.Nodes;
using Microsoft.OpenApi;
using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Swagger;
using TesterApp.Setup;

var builder = WebApplication.CreateBuilder(args);

// DI
DIContainer.Current
	.RegisterAll()
	.Verify();

// Swagger
builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen(x =>
	{
		x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.ApiKey,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
		});

		x.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecuritySchemeReference("Bearer"),
				new List<string>()
			}
		});

		var args = new SimplifyWebSwaggerArgs();

		var parameter = new OpenApiParameter
		{
			Name = "Accept-Language",
			In = ParameterLocation.Header,
			Description = "Language preference for the response.",
			Required = true,
			AllowEmptyValue = true,
			Example = "en-US",
			Schema = new OpenApiSchema
			{
				Default = "en-US",
				Enum =
				[
					(JsonNode)"en-US",
					(JsonNode)"ru-RU"
				]
			}
		};

		args.Parameters.Add(parameter);

		x.AddSimplifyWebSwagger(args);
	});

// App

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSimplifyWeb();

await app.RunAsync();