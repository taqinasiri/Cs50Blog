using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Services.Services.Identity;

public class RoleStoreService : RoleStore<Role, BlogDbContext, int, UserRole, RoleClaim>, IRoleStoreService
{
    public RoleStoreService(IUnitOfWork uow, IdentityErrorDescriber describer = null) : base((BlogDbContext)uow, describer) { }
}
