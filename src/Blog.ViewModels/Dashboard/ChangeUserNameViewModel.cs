using Blog.ViewModels.Application;

namespace Blog.ViewModels.Dashboard;

public class ChangeUserNameViewModel : GoogleReCaptchaModelBase
{
    public int Id { get; set; }

    [Display(Name = DisplayNames.UserName, Prompt = DisplayNames.UserName)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [StringLength(30, MinimumLength = 2, ErrorMessage = AttributesErrorMessages.StringLengthMessage)]
    [Remote("CheckUserName", "Account", null, HttpMethod = "POST", AdditionalFields = ViewModelConstants.AntiForgeryToken, ErrorMessage = AttributesErrorMessages.RemoteMessage)]
    [RegularExpression(RegularExpressions.UserName, ErrorMessage = AttributesErrorMessages.UserNameRegularExpressionMessage)]
    public string UserName { get; set; }
}
