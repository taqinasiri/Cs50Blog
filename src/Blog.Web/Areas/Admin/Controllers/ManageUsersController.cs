using Blog.DataLayer.Context;
using Blog.Entities;
using Blog.Entities.Identity;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Areas.Admin.Controllers;

[Authorize, Area(AreaConstants.Admin)]
public class ManageUsersController : Controller
{
    private readonly IUserManagerService _userManager;
    private readonly IRoleManagerService _roleManager;
    private readonly IUserInformationService _userInformationService;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    public ManageUsersController(IUserManagerService userManager, IRoleManagerService roleManager,IUserInformationService userInformationService,IUnitOfWork uow,IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _userInformationService = userInformationService;
        _uow = uow;
        _mapper= mapper;
    }

    public IActionResult Index(UserFiltersAndPaginationViewModel filters)
        => View(_userManager.GetUsersWithFilterAndPagination(filters));


    public IActionResult Statistics()
        => View(_userManager.GetUsersStatistics());

    public async Task<JsonResult> Details(int id)
        => Json(await _userManager.GetUserDetailsForAdminAsync(id));

    #region UserRoles

    public async Task<IActionResult> EditUser(int id)
    {
        var model = await _userManager.GetUserRolesToEditAsync(id);
        if (model is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserRolesViewModel model)
    {
        if (!ModelState.IsValid || !(await _roleManager.CheckRolesAsync(model.Roles)))
        {
            ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        var userInformation = await _userInformationService.GetUserInformationByUserIdAsync(model.Id);
        if (userInformation is null) return View("Error", PublicErrorMessages.SystemError);

        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRolesAsync(user, model.Roles);

        user.DisplayName = model.DisplayName;
        userInformation.Biography = model.Biography;
        userInformation.Website = model.Website;
        userInformation.Instagram = model.Instagram;
        userInformation.Twitter = model.Twitter;
        userInformation.Linkedin = model.Linkedin;

        if (model.NewAvatar is not null && model.NewAvatar.Length > 0)
        {
            var avatarName = Guid.NewGuid().ToString("N");
            var avatarExtension = Path.GetExtension(model.NewAvatar.FileName);
            if (user.Avatar != PublicConstantStrings.UserDefaultAvatar)
            {
                user.Avatar.RemoveImage(ImageFolders.Avatars);
            }
            model.NewAvatar.SaveImage(avatarName, avatarExtension, ImageFolders.Avatars);
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

        return RedirectToAction(nameof(Index));
    }
    #endregion

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeUserActive(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        user.IsActive = !user.IsActive;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return View("Error", PublicErrorMessages.SystemError);
        return RedirectToAction(nameof(Index));
    }
}
