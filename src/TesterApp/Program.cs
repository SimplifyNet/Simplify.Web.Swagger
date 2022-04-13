using Simplify.DI;
using Simplify.Web;
using Simplify.Web.Json.Model.Binding;
using Simplify.Web.Model;
using TesterApp.Setup;

var builder = WebApplication.CreateBuilder(args);

// DI

DIContainer.Current
	.RegisterAll()
	.Verify();

builder.Services.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddSimplifyWebSwagger();

// Configuration

HttpModelHandler.RegisterModelBinder<JsonModelBinder>();

// App

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSimplifyWebWithoutRegistrations();

app.Run();