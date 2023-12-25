using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Web.Filters;

public class DynamicAuthorizeActionFilter : IAsyncActionFilter
{

    private readonly string[] _authorizedAreas;
    public DynamicAuthorizeActionFilter(params string[] authorizedAreas)
    {
        _authorizedAreas = authorizedAreas;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.RouteData.Values.TryGetValue("area", out var area);
        bool isNext = true;
        if (area is not null && _authorizedAreas.Any(a => a == area.ToString()) &&
            context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.RouteData.Values.TryGetValue("action", out var action);
            context.RouteData.Values.TryGetValue("controller", out var controller);
            var permission = $"{area}|{controller}|{action}";

            var roleManager = context.HttpContext.RequestServices.GetService<IRoleManagerService>();
            var roles = context.HttpContext.User.Identity.GetRoles();

            var result = roleManager.RolesHasPermission(roles, permission);

            if (!result)
            {
                isNext = false;
                context.Result = new RedirectToActionResult("AccessDenied", "Account", new { area = "" });
            }
        }
        if(isNext) await next();
    }
}
