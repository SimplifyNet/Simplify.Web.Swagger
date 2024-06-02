using Asp.Versioning;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1.Users;

[Get("/api/v1/users")]
[ApiVersion("1.0")]
// [Produces("application/json")]
[ProducesResponse(StatusCodes.Status200OK, typeof(IList<UserViewModel>), "application/json")]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetMultipleController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke()
	{
		var items = new List<UserViewModel>
		{
			new()
			{
				UserName = "User 1",
				CreationTime = DateTime.Now
			},
			new()
			{
				UserName = "User 2",
				CreationTime = DateTime.Now.Subtract(TimeSpan.FromDays(1))
			}
		};

		return Json(items);
	}
}