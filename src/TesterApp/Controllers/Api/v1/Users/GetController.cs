using Asp.Versioning;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;
using TesterApp.ViewModels.Users;

namespace TesterApp.Controllers.Api.v1.Users;

[Get("/api/v1/users/{id}")]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status200OK, typeof(UserViewModel), "application/json")]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetController : Controller2
{
	public ControllerResponse Invoke(int id) =>
		 Json(new UserViewModel
		 {
			 UserName = $"User {id}",
			 CreationTime = DateTime.Now
		 });
}