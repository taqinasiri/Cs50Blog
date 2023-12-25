namespace Blog.ViewModels.Dashboard;
public class ChangePasswordViewModel
{
    [Display(Name = DisplayNames.CurrentPassword, Prompt = DisplayNames.CurrentPassword)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }
    [Display(Name = DisplayNames.NewPassword, Prompt = DisplayNames.NewPassword)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [RegularExpression(RegularExpressions.Password, ErrorMessage = AttributesErrorMessages.PasswordRegularExpressionMessage)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Display(Name = DisplayNames.ConfirmPassword, Prompt = DisplayNames.ConfirmPassword)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [Compare(nameof(NewPassword), ErrorMessage = AttributesErrorMessages.CompareMessage)]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}