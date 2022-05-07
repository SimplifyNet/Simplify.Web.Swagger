using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Json.Responses;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1
{
	[Get("/api/v1/testOut")]
	[Post("/api/v1/testOut")]
	[ApiVersion("1.0")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TestViewModel))]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public class TestOutController : Simplify.Web.Controller
	{
		public override ControllerResponse Invoke()
		{
			var model = new TestViewModel
			{
				Prop1 = "Hello",
				Prop2 = "World"
			};

			return new Json(model);
		}
	}
}