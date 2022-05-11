using System.Net.Mime;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;

namespace TesterApp.Controllers
{
	[Get("status")]
	[ProducesResponse(StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
	public class StatusController : Controller
	{
		public override ControllerResponse Invoke() => Content("Service is running!", MediaTypeNames.Text.Plain);
	}
}