using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Identity;
public class User : IdentityUser<int>
{
    public DateTime CreatedDateTime { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<UserClaim> UserClaims { get; set; }
    public virtual ICollection<UserLogin> UserLogins { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public virtual ICollection<UserToken> UserTokens { get; set; }
}