using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.ViewModels.Users;

public class SearchingUsersViewModels
{
    public List<UserForAdminViewModel> Users { get; set; }
    public UserFiltersAndPaginationViewModel filters { get; set; }
}

public class UserFiltersAndPaginationViewModel
{
    public int PagesCount { get; set; }

    [HiddenInput]
    public int CurrentPage { get; set; }

    [Display(Name = DisplayNames.UserName, Prompt = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Email, Prompt = DisplayNames.Email)]
    public string Email { get; set; }

    [Display(Name = DisplayNames.ActivationStatus, Prompt = DisplayNames.ActivationStatus)]
    public AllTrueFalseEnum ActiveStatus { get; set; }

    [Display(Name = DisplayNames.EmailConfirmationStatus, Prompt = DisplayNames.EmailConfirmationStatus)]
    public AllTrueFalseEnum EmailConfirmedStatus { get; set; }

    public List<SelectListItem> ActiveStatusItems
    {
        get  => SelectListsModel.AllTrueFalse(DisplayNames.All, DisplayNames.Active, DisplayNames.Inactive);
    }
    public List<SelectListItem> EmailConfirmedStatusItems
    {
        get => SelectListsModel.AllTrueFalse(DisplayNames.All, DisplayNames.Confirmed, DisplayNames.NotConfirmed);
    }
}

public class UserForAdminViewModel
{
    public int Id { get; set; }

    [Display(Name = DisplayNames.Avatar)]
    public string Avatar { get; set; }

    [Display(Name = DisplayNames.CreatedDateTime)]
    public DateTime CreatedDateTime { get; set; }

    [Display(Name = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Email)]
    public string Email { get; set; }

    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
}
