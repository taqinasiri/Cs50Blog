using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;
public class RoleClaim : IdentityRoleClaim<int>
{
    public virtual Role Role { get; set; }
}
