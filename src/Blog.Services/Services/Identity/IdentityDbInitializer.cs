using Blog.Common.Constants;
using Blog.Common.Extensions;
using Blog.ViewModels.Application;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blog.Services.Services.Identity;
public class IdentityDbInitializer : IIdentityDbInitializer
{
    private readonly SiteSettings _options;
    private readonly IUserManagerService _userManager;
    private readonly RoleManagerService _roleManager;
    private readonly IServiceScopeFactory _scopeFactory;

    public IdentityDbInitializer(
        IUserManagerService userManager,
        IServiceScopeFactory scopeFactory,
        RoleManagerService roleManager,
        IOptionsSnapshot<SiteSettings> adminUserSeedOptions)
    {
        _userManager = userManager;
        _scopeFactory = scopeFactory;
        _roleManager = roleManager;
        _options = adminUserSeedOptions.Value;
    }

    public void Initialize()
    => _scopeFactory.RunScopedService<BlogDbContext>(context =>
        {
            context.Database.Migrate();
        });

    public void SeedData()
    => _scopeFactory.RunScopedService<IIdentityDbInitializer>(identityDbSeedData =>
        {
            var addRolesResult = identityDbSeedData.SeedSystemRoles().Result;
            if (addRolesResult == IdentityResult.Failed()) throw new InvalidOperationException();

            var addAdminResult = identityDbSeedData.SeedAdminUserWithRole().Result;
            if (addAdminResult == IdentityResult.Failed()) throw new InvalidOperationException();
        });

    public async Task<IdentityResult> SeedAdminUserWithRole()
    {
        var adminUserSeed = _options.AdminUserSeed;

        var name = adminUserSeed.UserName;
        var email = adminUserSeed.Email;
        var password = adminUserSeed.Password;

        var adminUser = await _userManager.FindByNameAsync(name);
        if (adminUser is not null) return IdentityResult.Success;

        var adminRole = await _roleManager.FindByNameAsync(SystemRoles.Admin);
        if (adminRole is null)
        {
            adminRole = new Role
            {
                Name = SystemRoles.Admin,
            };
            var adminRoleResult = await _roleManager.CreateAsync(adminRole);
            if (adminRoleResult == IdentityResult.Failed()) return IdentityResult.Failed();
        }


        adminUser = new User
        {
            UserName = name,
            Email = email,
            EmailConfirmed = true,
            Avatar = PublicConstantStrings.UserDefaultAvatar,
            IsActive = true,
            DisplayName = name,
            CreatedDateTime = DateTime.Now,
        };

        var adminUserResult = await _userManager.CreateAsync(adminUser, password);
        if (adminUserResult == IdentityResult.Failed()) return IdentityResult.Failed();

        var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, adminRole.Name);
        if (addToRoleResult == IdentityResult.Failed()) return IdentityResult.Failed();

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> SeedSystemRoles()
    {
        var roles = typeof(SystemRoles).GetAllPublicConstantValues<string>();
        foreach (var role in roles)
        {
            var newRole =await _roleManager.FindByNameAsync(role);
            if (newRole is not null) continue;
            var result = await _roleManager.CreateAsync(new Role
            {
                Name = role,
            });
            if(result == IdentityResult.Failed()) return IdentityResult.Failed();
        }
        return IdentityResult.Success;
    }
}
