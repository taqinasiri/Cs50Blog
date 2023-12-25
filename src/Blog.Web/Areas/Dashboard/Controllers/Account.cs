using Blog.DataLayer.Context;
using Blog.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Areas.Dashboard.Controllers;

[Authorize, Area(AreaConstants.Dashboard)]
public class Account : Controller
{
    private readonly IUnitOfWork _uow;
    private readonly IUserManagerService _userManager;
    private readonly IUserInformationService _userInformationService;
    public Account(IUnitOfWork uow, IUserManagerService userManager, IUserInformationService userInformationService)
    {
        _uow = uow;
        _userManager = userManager;
        _userInformationService = userInformationService;
    }

    public IActionResult Index()
    {
        return View();
    }


    #region Edit Profile

    public async Task<IActionResult> EditProfile()
    {
        var model = await _userManager.GetUserToEditProfileAsync(User.Identity.GetUserId());
        if (model is null) return View("Error", PublicErrorMessages.SystemError);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userInformation = await _userInformationService.GetUserInformationByUserIdAsync(userId);
            if (user is null || userInformation is null) return View("Error", PublicErrorMessages.SystemError);

            user.DisplayName = model.DisplayName;
            userInformation.Biography = model.Biography;
            userInformation.Website = model.Website;
            userInformation.Instagram = model.Instagram;
            userInformation.Twitter = model.Twitter;
            userInformation.Linkedin = model.Linkedin;

            if (model.Avatar is not null && model.Avatar.Length > 0)
            {
                var avatarName = Guid.NewGuid().ToString("N");
                var avatarExtension = Path.GetExtension(model.Avatar.FileName);
                if (user.Avatar != PublicConstantStrings.UserDefaultAvatar)
                {
                    user.Avatar.RemoveImage(ImageFolders.Avatars);
                }
                model.Avatar.SaveImage(avatarName, avatarExtension, ImageFolders.Avatars);
                user.Avatar = avatarName + avatarExtension;
            }

            _userInformationService.Update(user.UserInformation);
            _uow.SaveChanges();
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }
        else ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
        return View(model);
    }
    #endregion

    #region Change Password
    public IActionResult ChangePassword() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return View("Error", PublicErrorMessages.SystemError);


            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }
        else ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
        return View(model);
    }
    #endregion

    #region Change UserName
    public async Task<IActionResult> ChangeUserName()
    {
        var model = await _userManager.GetUserForChangeUserNameAsync(User.Identity.GetUserId());
        if (model is null) return View("Error", PublicErrorMessages.SystemError);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserName(ChangeUserNameViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) return View("Error", PublicErrorMessages.SystemError);


            var result = await _userManager.SetUserNameAsync(user, model.UserName);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }
        else ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
        return View(model);
    }
    #endregion
}
