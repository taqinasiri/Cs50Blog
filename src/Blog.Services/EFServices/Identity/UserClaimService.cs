using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Blog.Services.Contracts.Identity;

namespace Blog.Services.EFServices.Identity;

public class UserClaimService : UserClaimsPrincipalFactory<User, Role>
{
    public UserClaimService(
        IUserManagerService userManagerService,
        IRoleManagerService roleManagerService,
        IOptions<IdentityOptions> optionsAccessor) : base(
            (UserManager<User>)userManagerService,
            (RoleManager<Role>)roleManagerService, optionsAccessor)
    {

    }

    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var pricipal = await base.CreateAsync(user);

        ((ClaimsIdentity)pricipal.Identity).AddClaims(new[]
        {
                new Claim(ClaimTypes.NameIdentifier,user.UserName)
            });

        return pricipal;
    }
}

