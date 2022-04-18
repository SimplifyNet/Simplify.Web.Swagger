using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;

namespace TesterApp.Controllers.Api.v1
{
	[Route("api/v{version:apiVersion}/weather-forecasts")]
	[Produces("application/json")]
	[ApiVersion("1.0")]
	[Get("/api/v1/testOut")]
	public class TestOutController : Simplify.Web.Controller
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public override ControllerResponse Invoke()
		{
			throw new NotImplementedException();
		}
	}
}