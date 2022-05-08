using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;

namespace TesterApp.Controllers.Api.v1.Users;

[Delete("/api/v1/users/{id:int}")]
[Authorize]
[ApiVersion("1.0")]
[Produces("application/text")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class DeleteController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke()
	{
		if (RouteParameters.id <= 0)
			return StatusCode(400, "User ID is invalid");

		if (RouteParameters.id > 100)
			return StatusCode(500, "Internal Server Error");

		return NoContent();
	}
}