using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;
public class UserLogin : IdentityUserLogin<int>
{
    public virtual User User { get; set; }
}

