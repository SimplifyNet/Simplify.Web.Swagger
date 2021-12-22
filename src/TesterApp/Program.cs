using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Json.Model.Binding;
using Simplify.Web.Model;
using TesterApp.Setup;

var builder = WebApplication.CreateBuilder(args);

DIContainer.Current
	.RegisterAll()
	.Verify();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

HttpModelHandler.RegisterModelBinder<JsonModelBinder>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseSimplifyWebWithoutRegistrations();

app.Run();