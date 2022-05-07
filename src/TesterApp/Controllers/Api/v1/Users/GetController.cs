using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Json.Responses;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1.Users;

[Get("/api/v1/users/{id:int}")]
[ApiVersion("1.0")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserViewModel))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class GetController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke() =>
		 new Json(new UserViewModel
		 {
			 UserName = "User 1",
			 CreationTime = DateTime.Now
		 });
}