using AutoMapper;
using Blog.Entities.Identity;
using Blog.ViewModels.Roles;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.Services.Services.Identity;

public class RoleManagerService : RoleManager<Role>, IRoleManagerService
{
    private readonly DbSet<Role> _roles;
    private readonly IMapper _mapper;
    public RoleManagerService(IRoleStoreService store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<RoleManagerService> logger,
        IUnitOfWork _uow,
        IMapper mapper) : base((RoleStore<Role, BlogDbContext, int, UserRole, RoleClaim>)store,
            roleValidators, keyNormalizer, errors, logger)
    {
        _roles = _uow.Set<Role>();
        _mapper = mapper;
    }

    public Task<EditRoleViewModel> GetRoleToEditAsync(int roleId)
        => _mapper.ProjectTo<EditRoleViewModel>(_roles).SingleOrDefaultAsync(r => r.Id == roleId);

    public RolesWithFilterAndPaginationViewModel GetRolesWithFilterAndPagination(string searchKey, int page, int take = 20)
    {
        var roles = _roles.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchKey))
            roles = roles.Where(r => r.Name.Contains(searchKey));

        var allRecordCount = roles.Count();
        var allPageCount = (int)Math.Ceiling((decimal)allRecordCount / take);
        if (page < 1) page = 1;
        if (page > allPageCount) page = allPageCount;
        var skip = (page - 1) * take;
        return new RolesWithFilterAndPaginationViewModel()
        {
            SearchKey = searchKey,
            CurrentPage = page,
            PagesCount = allPageCount,
            Roles = _mapper.ProjectTo<RoleForAdminViewModel>(roles.Skip(skip).Take(take)).ToList()
        };
    }


    public Task<Role> GetRoleToUpdateAsync(int roleId) => _roles
        .Include(r => r.RolePermissions).SingleOrDefaultAsync(r => r.Id == roleId);

    public Task<RoleDetailsViewModel> GetRoleDetailsById(int roleId)
        => _mapper.ProjectTo<RoleDetailsViewModel>(_roles).SingleOrDefaultAsync(r => r.Id == roleId);

    public List<string> GetAllRoleNames() => _roles.Select(r => r.Name).ToList();

    public async Task<bool> CheckRolesAsync(List<string> roles)
        => await _roles.CountAsync(r => roles.Contains(r.Name)) == roles.Count;

    public bool RolesHasPermission(List<string> roles, string permission)
    {
        foreach (var role in roles)
        {
            if (_roles.Any(r => r.Name == role && r.RolePermissions.Any(rp => rp.Permission == permission)))
                return true;
        }
        return false;
    }
    public bool RolesHasPermission(List<string> roles, string area, string controller = "Home", string action = "Index")
        => RolesHasPermission(roles, $"{area}|{controller}|{action}");
}
