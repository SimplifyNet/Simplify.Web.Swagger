﻿using Asp.Versioning;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;
using TesterApp.ViewModels;
using TesterApp.ViewModels.Users;

namespace TesterApp.Controllers.Api.v1.Users;

[Get("/api/v1/users/{id:int}")]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status200OK, typeof(UserViewModel), "application/json")]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke() =>
		 Json(new UserViewModel
		 {
			 UserName = $"User {RouteParameters.id}",
			 CreationTime = DateTime.Now
		 });
}