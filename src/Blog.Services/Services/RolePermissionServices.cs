using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Services;
public class RolePermissionServices : GenericService<RolePermission>, IRolePermissionServices
{
    private readonly DbSet<RolePermission> _rolePermissions;
    public RolePermissionServices(IUnitOfWork uow) : base(uow)
    {
        _rolePermissions = uow.Set<RolePermission>();
    }
}
