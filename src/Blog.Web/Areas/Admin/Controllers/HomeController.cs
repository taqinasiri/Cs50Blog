using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Areas.Admin.Controllers;

[Authorize(Roles = SystemRoles.Admin), Area(AreaConstants.Admin)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
