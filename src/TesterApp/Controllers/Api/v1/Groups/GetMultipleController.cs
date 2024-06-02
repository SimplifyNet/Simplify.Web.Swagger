using System.Net.Mime;
using Asp.Versioning;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Swagger;
using TesterApp.ViewModels.Groups;

namespace TesterApp.Controllers.Api.v1.Groups;

[Get("/api/v1/groups")]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status200OK, typeof(IList<GroupViewModel>), MediaTypeNames.Application.Json)]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetMultipleController : Controller
{
	public override ControllerResponse Invoke()
	{
		var languageCode = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

		var items = new List<GroupViewModel>();

		switch (languageCode)
		{
			case "ru":
				items.Add(new GroupViewModel { Name = "Группа 1" });
				items.Add(new GroupViewModel { Name = "Группа 2" });
				break;

			default:
				items.Add(new GroupViewModel { Name = "Group 1" });
				items.Add(new GroupViewModel { Name = "Group 2" });
				break;
		}

		// Items retrieve

		return Json(items);
	}
}