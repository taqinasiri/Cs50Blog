namespace Blog.ViewModels.Application;
public class SiteSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public GoogleAuthenticationModel GoogleAuthentication { get; set; }
    public AdminUserSeed AdminUserSeed { get; set; }
}
