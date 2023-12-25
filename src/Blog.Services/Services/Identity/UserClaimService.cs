using Blog.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Blog.Services.Services.Identity;
public class UserClaimService : UserClaimsPrincipalFactory<User, Role>
{
    public UserClaimService(
        IUserManagerService userManagerService,
        IRoleManagerService roleManagerService,
        IOptions<IdentityOptions> optionsAccessor) : base(
            (UserManager<User>)userManagerService,
            (RoleManager<Role>)roleManagerService, optionsAccessor)
    { }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var pricipal = await base.CreateAsync(user);
        ((ClaimsIdentity)pricipal.Identity).AddClaims(new[]
        {
            new Claim(IdentityClaimNames.DisplayName, user.DisplayName),
            new Claim(IdentityClaimNames.Avatar, user.Avatar),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        });
        return pricipal;
    }
}
