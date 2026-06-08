using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Swagger;
using TesterApp.Setup;
#if NET10_0
using Microsoft.OpenApi;
#else
using Microsoft.OpenApi.Models;
#endif

var builder = WebApplication.CreateBuilder(args);

// DI
DIContainer.Current.RegisterAll().Verify();

// Swagger
builder
	.Services.AddSimplifyWebSwaggerServices()
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(x =>
	{
		x.AddSecurityDefinition(
			"Bearer",
			new OpenApiSecurityScheme
			{
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				Description =
					"Input your Bearer token in this format - Bearer {your token here} to access this API",
			}
		);

		var swaggerArgs = new SimplifyWebSwaggerArgs
		{
			AcceptLanguageHeader = new AcceptLanguageHeaderArgs("en-US", "ru-RU", "kk-KZ"),
		};

		x.AddSimplifyWebSwagger(swaggerArgs);
	});

// App

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseSimplifyWeb();

await app.RunAsync();