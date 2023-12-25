using Blog.Entities;
using Blog.Entities.Identity;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;

namespace Blog.Web.Areas.Admin.Controllers;

[Authorize, Area(AreaConstants.Admin)]
public class ManageRolesController : Controller
{
    IRoleManagerService _roleManager;
    IMapper _mapper;
    public ManageRolesController(IRoleManagerService roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }
    public IActionResult Index(string filter, int page = 1)
        => View(_roleManager.GetRolesWithFilterAndPagination(filter, page));

    #region AddRole
    public IActionResult AddRole() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(AddRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
            return View(model);
        };
        var role = _mapper.Map<Role>(model);
        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        return RedirectToAction("Index");
    }
    #endregion
    #region EditRole
    public async Task<IActionResult> EditRole(int id)
    {
        var model = await _roleManager.GetRoleToEditAsync(id);
        if (model is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        //if(model.) //TODO : check system roles not edit permissions
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicErrorMessages.ModelStateErrorMessage);
            return View(model);
        };
        var role = await _roleManager.GetRoleToUpdateAsync(model.Id);
        if (role is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        role.Name = model.Name;
        role.RolePermissions.Clear();
        role.RolePermissions.AddRange(model.Permissions.Select(rp => new RolePermission()
        {
            Permission = rp,
        }));

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        return RedirectToAction("Index");
    }
    #endregion

    public async Task<IActionResult> RoleDetails(int id)
    {
        var model = await _roleManager.GetRoleDetailsById(id);
        if (model is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role is null) return View("Error", PublicErrorMessages.ErrorTryAgain);
        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded) return View("Error", PublicErrorMessages.SystemError);
        return RedirectToAction("Index");
    }

    public IActionResult RolePermissions()
        => View(typeof(ManageRolesController).Assembly.GetAreaControllerActionNames()
        .Where(m => m.Area == AreaConstants.Admin));
}
