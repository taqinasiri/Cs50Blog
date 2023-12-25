using Blog.ViewModels.Application;

namespace Blog.ViewModels.Account;

public class ResendConfirmationCodeViewModel : GoogleReCaptchaModelBase
{
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Display(Name = DisplayNames.UserName, Prompt = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Email, Prompt = DisplayNames.Email)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [RegularExpression(RegularExpressions.Email, ErrorMessage = AttributesErrorMessages.EmailAddressMessage)]
    [EmailAddress(ErrorMessage = AttributesErrorMessages.EmailAddressMessage)]
    public string Email { get; set; }
}