using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;

namespace TesterApp.Controllers.Api.v1.Groups;

[Patch("/api/v1/groups/{id}/rename")]
[Authorize]
[ApiVersion("1.0")]
[Produces("application/text")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class RenameController : Controller2
{
	public ControllerResponse Invoke(int id) =>
		id <= 0
			? StatusCode(400, "User ID is invalid")
			: NoContent();
}