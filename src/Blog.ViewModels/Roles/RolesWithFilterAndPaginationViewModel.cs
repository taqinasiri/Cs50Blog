namespace Blog.ViewModels.Users;

public class RolesWithFilterAndPaginationViewModel
{
    public List<RoleForAdminViewModel> Roles { get; set; }
    public int PagesCount { get; set; }

    [HiddenInput]
    public int CurrentPage { get; set; }

    [Display(Name = DisplayNames.Search, Prompt = DisplayNames.Search)]
    public string SearchKey { get; set; }
}

public class RoleForAdminViewModel
{
    public int Id { get; set; }
    [Display(Name = DisplayNames.RoleName)]
    public string Name { get; set; }
    [Display(Name = DisplayNames.UsersInRoleCount)]
    public int UsersInRoleCount { get; set; }
}