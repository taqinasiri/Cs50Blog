using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Areas.Dashboard.Controllers;
[Authorize, Area(AreaConstants.Dashboard)]
public class HomeController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}