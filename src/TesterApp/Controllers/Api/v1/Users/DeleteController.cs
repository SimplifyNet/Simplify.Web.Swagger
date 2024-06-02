using Asp.Versioning;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;

namespace TesterApp.Controllers.Api.v1.Users;

[Delete("/api/v1/users/{id:int}")]
[Authorize]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status204NoContent)]
[ProducesResponse(StatusCodes.Status400BadRequest)]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
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