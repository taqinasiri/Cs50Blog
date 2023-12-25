using System.ComponentModel;

namespace Blog.ViewModels.Roles;

public class RoleDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }


    [Display(Name = DisplayNames.Permissions)]
    public List<string> Permissions { get; set; }

    [Display(Name = DisplayNames.UsersInRoleCount)]
    public int UsersInRoleCount { get; set; }

}
