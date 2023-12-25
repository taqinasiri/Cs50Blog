using System.Collections.Generic;

namespace Blog.ViewModels.Users;
public class UsersStatisticsViewModel
{
    [Display(Name = DisplayNames.UsersCount)]
    public int UsersCount { get; set;}

    [Display(Name = DisplayNames.ActiveUsersCount)]
    public int ActiveUsersCount { get; set; }

    [Display(Name = DisplayNames.ConfirmedEmailUsersCount)]
    public int ConfirmedEmailUsersCount { get; set; }

    [Display(Name = DisplayNames.ConfirmedUsersCount)]
    public int ConfirmedUsersCount { get; set; }

    [Display(Name = DisplayNames.RegistrationLastThirtyDaysCount)]
    public int RegistrationLastThirtyDaysCount { get; set; }

    [Display(Name = DisplayNames.RegistrationLastSevenDaysCount)]
    public int RegistrationLastSevenDaysCount { get; set; }
    [Display(Name = DisplayNames.ExternalLogins)]
    public int ExternalLogins { get; set; }
}
