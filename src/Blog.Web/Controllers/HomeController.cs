using System.Diagnostics;
using Blog.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Controllers;

public class HomeController : Controller
{
    private readonly IPostService _postService;

    public HomeController(IPostService postService)
    {
        _postService = postService;
    }

    public async Task<IActionResult> Index() => View(await _postService.GetPostsAsync());
    [Route("Post/{id}")]
    public async Task<IActionResult> Post(int id)
    {
        var model = await _postService.GetPostDetailsAsync(id);
        if (model is null) return View("NotFound");
        return View(model);
    }

    public IActionResult AboutMe() => View();

}
