using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Web.Filters;
public class NotAuthorize : ActionFilterAttribute
{
    private readonly string _redirectUrl;
    public NotAuthorize(string redirectUrl = "/") => _redirectUrl = redirectUrl;

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new RedirectResult(_redirectUrl);
        }
        base.OnResultExecuting(context);
    }
}
