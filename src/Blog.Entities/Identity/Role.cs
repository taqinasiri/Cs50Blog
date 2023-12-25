using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;
public class Role : IdentityRole<int>
{
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}

