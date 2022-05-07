using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1.Users;

[Post("/api/v1/users")]
[ApiVersion("1.0")]
[Produces("application/text")]
public class CreateController : AsyncController<UserAddViewModel>
{
	public override async Task<ControllerResponse> Invoke()
	{
		await ReadModelAsync();

		return Content($"User created at {DateTime.Now.ToLongTimeString()}'");
	}
}