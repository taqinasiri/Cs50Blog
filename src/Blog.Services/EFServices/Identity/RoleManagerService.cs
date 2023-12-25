using Blog.Services.Contracts.Identity;
using Blog.DataLayer.Context;
using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Services.EFServices.Identity;

public class RoleManagerService : RoleManager<Role>, IRoleManagerService
{
    private readonly DbSet<Role> _roles;
    public RoleManagerService(IRoleStoreService store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManagerService> logger,
        IUnitOfWork _uow) : base((RoleStore<Role, BlogDbContext, int, UserRole, RoleClaim>)store,
            roleValidators, keyNormalizer, errors, logger)
    {
        _roles = _uow.Set<Role>();
    }

    public async Task<bool> CheckRolesAsync(List<string> roles)
    {
        var selectedRoles = await _roles.LongCountAsync(r => roles.Contains(r.Name));
        return roles.Count == selectedRoles;
    }
    public Task<Role> RoleToDelete(int id) => _roles.SingleOrDefaultAsync(r => r.Id == id && !r.UserRoles.Any());
}

