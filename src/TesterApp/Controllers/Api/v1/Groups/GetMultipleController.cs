using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Json.Responses;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1.Groups;

[Get("/api/v1/groups")]
[ApiVersion("1.0")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<GroupViewModel>))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class GetMultipleController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke()
	{
		var items = new List<GroupViewModel>
		{
			new GroupViewModel
			{
				Name = "Group 1"
			},
			new GroupViewModel
			{
				Name = "Group 2"
			}
		};

		// Items retrieve

		return new Json(items);
	}
}