﻿using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Simplify.Web;
using Simplify.Web.Attributes;
using Simplify.Web.Json.Responses;
using Simplify.Web.Swagger;
using TesterApp.ViewModels;

namespace TesterApp.Controllers.Api.v1.Groups;

[Get("/api/v1/groups")]
[ApiVersion("1.0")]
[ProducesResponse(StatusCodes.Status200OK, typeof(IList<GroupViewModel>), MediaTypeNames.Application.Json)]
[ProducesResponse(StatusCodes.Status500InternalServerError)]
public class GetMultipleController : Simplify.Web.Controller
{
	public override ControllerResponse Invoke()
	{
		var languageCode = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

		var items = new List<GroupViewModel>();

		switch (languageCode)
		{
			case "ru":
				items.Add(new() {Name = "Группа 1"});
				items.Add(new() {Name = "Группа 2"});
				break;
			default:
				items.Add(new() {Name = "Group 1"});
				items.Add(new() {Name = "Group 2"});
				break;
		}

		// Items retrieve

		return new Json(items);
	}
}