using System.Net.Mime;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;

namespace TesterApp.Controllers
{
	[Get("string-display/{Str}")]
	[ProducesResponse(StatusCodes.Status200OK, MediaTypeNames.Text.Plain)]
	public class StringDisplayController : Controller
	{
		public override ControllerResponse Invoke() => Content("User string: " + RouteParameters.Str);
	}
}