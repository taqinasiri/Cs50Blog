using Blog.Common.Mvc;

namespace Blog.ViewModels.Application;
public class GoogleReCaptchaModelBase
{
    [Required]
    [GoogleReCaptchaValidation]
    [BindProperty(Name = "g-recaptcha-response")]
    public string GoogleReCaptchaResponse { get; set; }
}
