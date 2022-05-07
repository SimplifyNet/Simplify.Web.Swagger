using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1
{
	[Post("/api/v1/testIn")]
	[ApiVersion("1.0")]
	[Produces("application/text")]
	public class TestInController : AsyncController<TestViewModel>
	{
		public override async Task<ControllerResponse> Invoke()
		{
			await ReadModelAsync();

			return Content($"Prop1 = '{Model.Prop1}', Prog2 = '{Model.Prop2}'");
		}
	}
}