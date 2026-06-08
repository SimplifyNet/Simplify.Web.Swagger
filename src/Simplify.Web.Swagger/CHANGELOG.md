# Changelog

## [2.0.0] - 2026-06-08

### Added

- .NET 10.0 explicit support
- `AcceptLanguageHeaderArgs` — configures an `Accept-Language` header parameter added to all Swagger operations
- `SimplifyWebSwaggerArgs.AcceptLanguageHeader` — enables Accept-Language header with supported languages list
- `SimplifyWebSwaggerArgs.SecuritySchemeName` — explicit override for security scheme name; when `null` (default), all schemes registered via `AddSecurityDefinition` are applied automatically to authorized operations
- Route parameter types auto-detection from controller `Invoke`/`InvokeAsync` method signatures (typed path parameters in Swagger schema)
- `AddSimplifyWebSwaggerServices` extension method — registers PascalCase `ISerializerDataContractResolver` before `AddSwaggerGen`

### Changed

- Security requirements on authorized operations are now auto-detected from registered `AddSecurityDefinition` schemes; no manual `SecuritySchemeName` configuration needed

### Dependencies

- Simplify.Web bump to 5.2
- Asp.Versioning.Mvc bump to 8.1.1
- Swashbuckle.AspNetCore.SwaggerGen bump to 10.2.1

## [1.2.0] - 2025-10-10

### Removed

.NET 6.0 support
.NET Standard 2.0 support

### Added

.NET 9.0 explicit support
.NET 8.0 explicit support

### Dependencies

- Simplify.Web bump to 5.1
- Asp.Versioning.Mvc bump to 8.1
- Swashbuckle.AspNetCore.SwaggerGen bump to 9.0.6

## [1.1.0] - 2025-10-02

### Added

- ContentType field in RequestBody attribute

## [1.0.0] - 2024-06-05

### Dependencies

- Simplify.Web bump to 5.0
- Switch to Asp.Versioning.Mvc 6.4.1 for .NET 6 package, .NET Standard 2.0 package remain Microsoft.AspNetCore.Mvc.Versioning 4.1 dependant
- Swashbuckle.AspNetCore.SwaggerGen bump to 6.6.2

## [0.4.1] - 2023-12-22

### Fixed

- Error while passing null args to AddSimplifyWebSwagger

## [0.4] - 2023-12-21

### Added

- Response body display and usage

## [0.3] - 2023-12-20

### Added

- Request body display and usage
- SimplifyWebSwaggerArgs
- Parameters display and string parameters execution

### Changed

- Authorization display

### Dependencies

- Simplify.Web bump to 4.8.1
- Swashbuckle.AspNetCore.SwaggerGen bump to 6.5

## [0.2] - 2022-06-14

### Added

- Parameters display
- Authorization display

### Dependencies

- Simplify.Web bump to 4.6
- Microsoft.AspNetCore.Mvc.Versioning bump to 4.1

## [0.1] - 2022-05-11

### Added

- Initial version with controllers list including response types handling
