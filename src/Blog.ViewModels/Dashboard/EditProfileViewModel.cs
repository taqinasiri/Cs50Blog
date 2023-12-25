namespace Blog.ViewModels.Dashboard;
public class EditProfileViewModel
{
    [HiddenInput]
    public int Id { get; set; }

    [Display(Name = DisplayNames.DisplayName, Prompt = DisplayNames.DisplayName)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    [StringLength(100, MinimumLength = 2, ErrorMessage = AttributesErrorMessages.StringLengthMessage)]
    public string DisplayName { get; set; }

    [Display(Name = DisplayNames.Avatar)]
    //[FileNotEmpty(DisplayNames.Avatar)]
    //[IsImage(DisplayNames.Avatar)]
    [MaxFileSize(DisplayNames.Avatar, 1)]
    public IFormFile Avatar { get; set; }

    [Display(Name = DisplayNames.Biography,Prompt = DisplayNames.Biography)]
    [MaxLength(512,ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Biography { get; set; }

    [Display(Name = DisplayNames.Website, Prompt = DisplayNames.Website)]
    [MaxLength(50, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    [Url(ErrorMessage = AttributesErrorMessages.ValidUrl)]
    public string Website { get; set; }

    [Display(Name = DisplayNames.Instagram, Prompt = DisplayNames.Instagram)]
    [MaxLength(30, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Instagram { get; set; }

    [Display(Name = DisplayNames.Twitter, Prompt = DisplayNames.Twitter)]
    [MaxLength(15, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Twitter { get; set; }

    [Display(Name = DisplayNames.Linkedin, Prompt = DisplayNames.Linkedin)]
    [MaxLength(100, ErrorMessage = AttributesErrorMessages.MaxLengthMessage)]
    public string Linkedin { get; set; }
}
