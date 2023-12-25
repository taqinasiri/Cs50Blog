namespace Blog.ViewModels.Users;

//For Admin panel
public class UserDetailsViewModel
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
    public List<string> ExternalLogins { get; set; }
}
