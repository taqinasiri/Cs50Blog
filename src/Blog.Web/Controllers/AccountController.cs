using Blog.Common.Mvc;
using Blog.Entities.Identity;
using Blog.Services;
using Blog.Services.Contracts;
using Blog.Services.Contracts.Identity;
using Blog.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;
using Blog.Common.Constants;
using Blog.Services.EFServices.Identity;

namespace Blog.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserManagerService _userManager;
    private readonly IViewRendererService _viewRenderer;
    private readonly ISignInManagerService _signInManager;
    private readonly IEmailSenderService _emailSender;

    public AccountController(IUserManagerService userManager, IViewRendererService viewRenderer, ISignInManagerService signInManager, IEmailSenderService emailSender)
    {
        _userManager = userManager;
        _viewRenderer = viewRenderer;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public IActionResult AccessDenied() => View();
    #region Register And Confirmation

    public IActionResult Register() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                CreatedDateTime = DateTime.Now,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //Send Email
                var body = await _viewRenderer.RenderViewToStringAsync(
                    "~/Views/EmailTemplates/_ActivationUserEmailTemplate.cshtml",
                    new RegisterEmailConfirmationViewModel()
                    {
                        UserName = model.UserName,
                        ActivationCode = activationCode,
                        CreatedDateTime = user.CreatedDateTime.ToString(),
                    });
                await _emailSender.SendEmailAsync(model.Email, "Confirmation account", body);
                return View("RegisterSuccess");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        }

        return View(model);
    }

    public async Task<IActionResult> ConfirmationAccount(string userName, string code)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code)) return View("Error");

        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) return View("NotFound");

        var result = await _userManager.ConfirmEmailAsync(user, code);
        return View(result.Succeeded ? nameof(ConfirmationAccount) : "Error");
    }
    #endregion

    #region Login And Logout

    public IActionResult Login(string returnUrl)
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string returnUrl, LoginViewModel model)
    {
        if (User.Identity.IsAuthenticated) return RedirectToAction(nameof(HomeController.Index), "Home");
        ViewData["ReturnUrl"] = returnUrl;
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null) ModelState.AddModelError(string.Empty, "Username or email not valid");
            else if (!await _userManager.IsEmailConfirmedAsync(user)) ModelState.AddModelError(string.Empty, "first confirm email");
            else if (!user.IsActive) ModelState.AddModelError(string.Empty, "this account deActive");
            else
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                ModelState.AddModelError(string.Empty, "Username or email not valid");
            }
        }
        else ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;
        await _signInManager.SignOutAsync();
        if (user is not null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    #endregion

    #region Forgot And Reset Password

    public IActionResult ForgotPassword() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null) return View("ForgotPasswordConfirmation");
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            ModelState.AddModelError(string.Empty, "first confirm email");
            return View();
        }

        var resetPasswordCode = await _userManager.GeneratePasswordResetTokenAsync(user);
        //Send Email
        var body = await _viewRenderer.RenderViewToStringAsync(
            "~/Views/EmailTemplates/_ForgotPasswordEmailTemplate.cshtml",
            new ForgotPasswordEmailViewModel()
            {
                UserName = user.UserName,
                ResetPasswordCode = resetPasswordCode
            });
        await _emailSender.SendEmailAsync(user.Email, "Reset Password", body);
        return View("ForgotPasswordConfirmation");
    }

    public IActionResult ResetPassword(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return View("Error");
        ViewData["Token"] = code;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        ViewData["Token"] = model.Token;
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty,PublicConstantStrings.ModelStateErrorMessage);
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null) return View("ResetPasswordConfirmation");
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded) return View("ResetPasswordConfirmation");
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty,error.Description);
        }
        return View(model);
    }
    #endregion

    #region Remotes
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckUserName(string userName, int? id)
    {
        if (User.Identity.IsAuthenticated && id is not null)
        {
            var currentUser = await _userManager.FindByIdAsync(id.ToString());
            if (string.Equals(currentUser.UserName, userName, StringComparison.OrdinalIgnoreCase)) return Json(true);
        }
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) return Json(true);
        return Json(false);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckEmail(string email, int? id)
    {
        if (User.Identity.IsAuthenticated && id is not null)
        {
            var currentUser = await _userManager.FindByIdAsync(id.ToString());
            if (string.Equals(currentUser.Email, email, StringComparison.OrdinalIgnoreCase)) return Json(true);
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Json(true);
        return Json(false);
    }
    #endregion
}