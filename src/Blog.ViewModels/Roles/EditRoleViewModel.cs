using Blog.Common.Constants;

namespace Blog.ViewModels.Roles;
public class EditRoleViewModel
{
    [HiddenInput]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public int Id { get; set; }

    [Display(Name = DisplayNames.Title, Prompt = DisplayNames.Title)]
    [Required(ErrorMessage = AttributesErrorMessages.RequiredMessage)]
    public string Name { get; set; }

    [Display(Name = DisplayNames.Permissions)]
    public List<string> Permissions { get; set; }

}
