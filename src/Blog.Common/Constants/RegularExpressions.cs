namespace Blog.Common.Constants;
public static class RegularExpressions
{
    public const string Password = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[_#?!@$%^&*-]).{8,}$";
    public const string Email = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";
    public const string UserName = @"^([\w\.]+)$";
}
