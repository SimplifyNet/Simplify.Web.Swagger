# Simplify.Web.Swagger

[![Nuget Version](https://img.shields.io/nuget/v/Simplify.Web.Swagger)](https://www.nuget.org/packages/Simplify.Web.Swagger/)
[![Nuget Download](https://img.shields.io/nuget/dt/Simplify.Web.Swagger)](https://www.nuget.org/packages/Simplify.Web.Swagger/)
[![Build Package](https://github.com/SimplifyNet/Simplify.Web.Swagger/actions/workflows/build.yml/badge.svg)](https://github.com/SimplifyNet/Simplify.Web.Swagger/actions/workflows/build.yml)
[![Libraries.io dependency status for latest release](https://img.shields.io/librariesio/release/nuget/Simplify.Web.Swagger)](https://libraries.io/nuget/Simplify.Web.Swagger)
[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/SimplifyNet/Simplify.Web.Swagger)](https://www.codefactor.io/repository/github/simplifynet/Simplify.Web.Swagger)
![Platform](https://img.shields.io/badge/platform-.NET%206.0%20%7C%20.NET%20Standard%202.0-lightgrey)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen)](http://makeapullrequest.com)

`Simplify.Web.Swagger` is a package which provides Swagger generation for [Simplify.Web](https://github.com/SimplifyNet/Simplify.Web) web-framework controllers.

## Quick Start

1. Add `Swashbuckle.AspNetCore.SwaggerGen` and `Swashbuckle.AspNetCore.SwaggerUI` packages to your project

1. Add `AddSimplifyWebSwagger` in `AddSwaggerGen` registration and `Simplify.Web` controllers will be scanned when generation Swagger.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
 .AddSwaggerGen(x => x.AddSimplifyWebSwagger());
```

3. Use swagger as in regular ASP.NET Core project

```csharp
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSimplifyWebWithoutRegistrations();

app.Run();
```

4. Add controller Swagger attributes (if needed)

```csharp
[Get("/api/v1/users/{id:int}")]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status200OK, "application/json")]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetController : Simplify.Web.Controller
{
 ...
}
```

5. After application started go to <http://localhost:5000/swagger/index.html> or <http://localhost:5000/swagger/swagger.json> to see generated Swagger

## Contributing

There are many ways in which you can participate in the project. Like most open-source software projects, contributing code is just one of many outlets where you can help improve. Some of the things that you could help out with are:

- Documentation (both code and features)
- Bug reports
- Bug fixes
- Feature requests
- Feature implementations
- Test coverage
- Code quality
- Sample applications

## Related Projects

Additional extensions to Simplify.Web live in their own repositories on GitHub. For example:

- [Simplify.Web.Json](https://github.com/SimplifyNet/Simplify.Web.Json) - JSON serialization/deserialization
- [Simplify.Web.Multipart](https://github.com/SimplifyNet/Simplify.Web.Multipart) - multipart form model binder
- [Simplify.Web.MessageBox](https://github.com/SimplifyNet/Simplify.Web.MessageBox) - non-interactive server side message box
- [Simplify.Web.Templates](https://github.com/SimplifyNet/Simplify.Web.Templates) - Visual studio project templates

## License

Licensed under the GNU LESSER GENERAL PUBLIC LICENSE
