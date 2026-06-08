using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Simplify.Web.Controllers.Meta;
using Simplify.Web.Controllers.Meta.MetaStore;
using Simplify.Web.Controllers.Meta.Routing;
using Simplify.Web.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
#if NET10_0
using Microsoft.OpenApi;
using NetHttpMethod = System.Net.Http.HttpMethod;
#else
using Microsoft.OpenApi.Models;
#endif

namespace Simplify.Web.Swagger;

/// <summary>
/// Provides the controller action factory.
/// </summary>
public static class ControllerActionsFactory
{
    private static readonly IReadOnlyCollection<
        KeyValuePair<string, string>
    > ResponseDescriptionMap =
    [
        new KeyValuePair<string, string>("1\\d{2}", "Information"),
        new KeyValuePair<string, string>("201", "Created"),
        new KeyValuePair<string, string>("202", "Accepted"),
        new KeyValuePair<string, string>("204", "No Content"),
        new KeyValuePair<string, string>("2\\d{2}", "Success"),
        new KeyValuePair<string, string>("304", "Not Modified"),
        new KeyValuePair<string, string>("3\\d{2}", "Redirect"),
        new KeyValuePair<string, string>("400", "Bad Request"),
        new KeyValuePair<string, string>("401", "Unauthorized"),
        new KeyValuePair<string, string>("403", "Forbidden"),
        new KeyValuePair<string, string>("404", "Not Found"),
        new KeyValuePair<string, string>("405", "Method Not Allowed"),
        new KeyValuePair<string, string>("406", "Not Acceptable"),
        new KeyValuePair<string, string>("408", "Request Timeout"),
        new KeyValuePair<string, string>("409", "Conflict"),
        new KeyValuePair<string, string>("429", "Too Many Requests"),
        new KeyValuePair<string, string>("4\\d{2}", "Client Error"),
        new KeyValuePair<string, string>("5\\d{2}", "Server Error"),
        new KeyValuePair<string, string>("default", "Error"),
    ];

    /// <summary>
    /// Gets the remove prefixes.
    /// </summary>
    /// <value>
    /// The remove prefixes.
    /// </value>
    public static IList<string> RemovePrefixes { get; } = ["Controllers.", "Api.v1."];

    /// <summary>
    /// Creates the controller actions from controllers metadata.
    /// </summary>
    /// <param name="context">The context.</param>
    public static IEnumerable<ControllerAction> CreateControllerActionsFromControllersMetaData(
        DocumentFilterContext context
    ) =>
        ControllersMetaStore.Current.RoutedControllers.SelectMany(item =>
            CreateControllerActions(item, context)
        );

    private static IEnumerable<ControllerAction> CreateControllerActions(
        IControllerMetadata item,
        DocumentFilterContext context
    ) =>
        item.ExecParameters!.Routes.Select(x =>
            CreateControllerAction(x.Key, x.Value, item, context)
        );

    private static ControllerAction CreateControllerAction(
        HttpMethod method,
        IControllerRoute route,
        IControllerMetadata item,
        DocumentFilterContext context
    ) =>
        new()
        {
            Type = HttpMethodToOperationType(method),
            ControllerRoute = route,
            Names = CreateNames(item.ControllerType),
            Responses = CreateResponses(item.ControllerType, context),
            RequestBody = CreateRequestBody(item.ControllerType, context),
            IsAuthorizationRequired = item.Security is { IsAuthorizationRequired: true },
            RouteParameterTypes = GetRouteParameterTypes(item.ControllerType),
        };

    private static ControllerActionNames CreateNames(Type controllerType) =>
        CreateNames(
            controllerType.FullName
                ?? throw new InvalidOperationException("controllerType.FullName is null")
        );

    private static ControllerActionNames CreateNames(string name)
    {
        var src = FormatNameSource(name);

        var index = src.LastIndexOf("/");

        return index == -1
            ? new ControllerActionNames(src, src)
            : new ControllerActionNames(src, src.Substring(0, index), src.Substring(index + 1));
    }

    private static string FormatNameSource(string str)
    {
        foreach (var prefix in RemovePrefixes)
        {
            var prefixIndex = str.IndexOf(prefix);

            if (prefixIndex == -1)
                continue;

            str = str.Substring(prefixIndex + prefix.Length);
        }

        str = str.Replace(".", "/");

        if (str.EndsWith("Controller"))
            str = str.Substring(0, str.LastIndexOf("Controller"));

        return str;
    }

#if NET10_0
    private static NetHttpMethod HttpMethodToOperationType(HttpMethod method) =>
        method switch
        {
            HttpMethod.Get => NetHttpMethod.Get,
            HttpMethod.Post => NetHttpMethod.Post,
            HttpMethod.Put => NetHttpMethod.Put,
            HttpMethod.Patch => NetHttpMethod.Patch,
            HttpMethod.Delete => NetHttpMethod.Delete,
            HttpMethod.Options => NetHttpMethod.Options,
            _ => NetHttpMethod.Get,
        };
#else
    private static OperationType HttpMethodToOperationType(HttpMethod method) =>
        method switch
        {
            HttpMethod.Get => OperationType.Get,
            HttpMethod.Post => OperationType.Post,
            HttpMethod.Put => OperationType.Put,
            HttpMethod.Patch => OperationType.Patch,
            HttpMethod.Delete => OperationType.Delete,
            HttpMethod.Options => OperationType.Options,
            _ => OperationType.Get,
        };
#endif

    private static OpenApiRequestBody CreateRequestBody(
        Type controllerType,
        DocumentFilterContext context
    )
    {
        var request = new OpenApiRequestBody();
        var attributes = controllerType.GetCustomAttributes(typeof(RequestBodyAttribute), false);

        if (attributes.Length <= 0)
            return request;

        var item = (RequestBodyAttribute)attributes[0];

        request.Content = new Dictionary<string, OpenApiMediaType>
        {
            [item.ContentType] = new()
            {
                Schema = context.SchemaGenerator.GenerateSchema(
                    item.Model,
                    context.SchemaRepository
                ),
            },
        };

        return request;
    }

    private static IDictionary<int, OpenApiResponse> CreateResponses(
        Type controllerType,
        DocumentFilterContext context
    ) =>
        controllerType
            .GetCustomAttributes(typeof(ProducesResponseAttribute), false)
            .Cast<ProducesResponseAttribute>()
            .ToDictionary(item => item.StatusCode, item => CreateResponse(item, context));

    private static OpenApiResponse CreateResponse(
        ProducesResponseAttribute producesResponse,
        DocumentFilterContext context
    )
    {
        var response = new OpenApiResponse
        {
            Description = ResponseDescriptionMap
                .FirstOrDefault(
                    (entry) => Regex.IsMatch(producesResponse.StatusCode.ToString(), entry.Key)
                )
                .Value,
        };

        foreach (var item in producesResponse.ContentTypes.Distinct())
        {
#if NET10_0
            response.Content ??= new Dictionary<string, OpenApiMediaType>();
#endif
            response.Content.Add(
                item,
                producesResponse.Type is null
                    ? new OpenApiMediaType()
                    : new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(
                            producesResponse.Type,
                            context.SchemaRepository
                        ),
                    }
            );
        }

        return response;
    }

    private static IDictionary<string, Type> GetRouteParameterTypes(Type controllerType)
    {
        var method = controllerType.GetMethod("Invoke") ?? controllerType.GetMethod("InvokeAsync");

        if (method is null)
            return new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        return method
            .GetParameters()
            .Where(p => p.Name != null)
            .ToDictionary(p => p.Name!, p => p.ParameterType, StringComparer.OrdinalIgnoreCase);
    }
}
