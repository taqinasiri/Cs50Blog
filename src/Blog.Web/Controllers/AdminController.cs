using Blog.Common.Constants;
using Blog.Common.Extensions;
using Blog.DataLayer.Context;
using Blog.Entities;
using Blog.Entities.Identity;
using Blog.Services.Contracts;
using Blog.Services.Contracts.Identity;
using Blog.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace Blog.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserManagerService _userManager;
        private readonly IPostService _postService;
        protected readonly IUnitOfWork _uow;

        public AdminController(IUserManagerService userManager, IPostService postService, IUnitOfWork uow)
        {
            _userManager = userManager;
            _postService = postService;
            _uow = uow;
        }

        public IActionResult Index() => View();

        #region Users
        public async Task<IActionResult> Users() => View(await _userManager.UsersPreview());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return View("NotFound");
            if (user.Id == 1) return View("Error");
            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);
            await _userManager.UpdateSecurityStampAsync(user);
            if (!result.Succeeded) return View("Error");
            return RedirectToAction(nameof(Users));
        }
        #endregion

        #region Posts

        public async Task<IActionResult> AllPosts() => View(await _postService.GetPostsListAsync());

        #region Add

        public IActionResult AddPost() => View();
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(AddPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
                return View(model);
            }

            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content,
                CreateDate = DateTime.Now,
                CoverImage = "Default.jpg"
            };

            if (model.Cover is not null)
            {
                string coverName = Guid.NewGuid().ToString("N");
                var imageExtension = Path.GetExtension(model.Cover.FileName);
                post.CoverImage = coverName + imageExtension;
                model.Cover.SaveImage(coverName, imageExtension, "covers");
            }

            await _postService.AddAsync(post);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(AllPosts));
        }

        #endregion

        #region Edit

        public async Task<IActionResult> EditPost(int postId)
        {
            var model = await _postService.GetPostForEditAsync(postId);
            if (model is null) return View("NotFound");
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditPostViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var post = await _postService.FindByIdAsync(model.Id);
            if (post is null) return View("NotFound");

            post.Title = model.Title;
            post.Content = model.Content;
            if (model.Cover is not null)
            {
                if (post.CoverImage != "Default.jpg")
                {
                    WorkWithImages.RemoveImage(post.CoverImage, "covers");
                }

                string coverName = Guid.NewGuid().ToString("N");
                var imageExtension = Path.GetExtension(model.Cover.FileName);
                post.CoverImage = coverName + imageExtension;
                model.Cover.SaveImage(coverName, imageExtension, "covers");
            }

            _postService.Update(post);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(AllPosts));
        }

        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var post = await _postService.FindByIdAsync(postId);
            if (post is null) return View("Error");
            if (post.CoverImage != "Default.jpg")
            {
                WorkWithImages.RemoveImage(post.CoverImage, "covers");
            }
            _postService.Remove(post);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(AllPosts));
        }
        #endregion
    }
}
