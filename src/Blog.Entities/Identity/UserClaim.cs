using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;

public class UserClaim : IdentityUserClaim<int>
{
    public virtual User User { get; set; }
}

