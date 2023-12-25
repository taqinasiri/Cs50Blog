using Microsoft.AspNetCore.Identity;

namespace Blog.Services.Contracts.Identity;
public interface IIdentityDbInitializer
{
    /// <summary>
    /// Applies any pending migrations for the context to the database.
    /// Will create the database if it does not already exist.
    /// </summary>
    void Initialize();
    void SeedData();

    Task<IdentityResult> SeedSystemRoles();
    Task<IdentityResult> SeedAdminUserWithRole();
}
