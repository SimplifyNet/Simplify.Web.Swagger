using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Json.Model.Binding;
using Simplify.Web.Model;
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

		x.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer",
					},
					Scheme = "Bearer",
					Name = "Bearer",
					In = ParameterLocation.Header,
				},
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
			Example = new OpenApiString("en-US"),
			Schema = new OpenApiSchema
			{
				Default = new OpenApiString("en-US"),
				Enum = new List<IOpenApiAny>
				{
					new OpenApiString("en-US"),
					new OpenApiString("ru-RU"),
				}
			}
		};

		args.Parameters.Add(parameter);

		x.AddSimplifyWebSwagger(args);
	});

// Configuration

HttpModelHandler.RegisterModelBinder<JsonModelBinder>();

// App

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSimplifyWebWithoutRegistrations();

app.Run();