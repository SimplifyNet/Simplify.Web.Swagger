using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Json.Model.Binding;
using Simplify.Web.Model;
using TesterApp.Setup;

DIContainer.Current
.RegisterAll()
.Verify();

HttpModelHandler.RegisterModelBinder<JsonModelBinder>();

var app = WebApplication.CreateBuilder(args).Build();

app.UseSimplifyWebWithoutRegistrations();

app.Run();