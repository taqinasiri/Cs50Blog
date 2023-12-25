using AutoMapper;
using Blog.ViewModels;
using Blog.ViewModels.Dashboard;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.Services.Services.Identity;
public class UserManagerService : UserManager<User>, IUserManagerService
{
    private readonly DbSet<User> _users;

    private readonly IMapper _mapper;
    public UserManagerService(IUserStoreService store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<User> passwordHasher,
    IEnumerable<IUserValidator<User>> userValidators,
    IEnumerable<IPasswordValidator<User>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManagerService> logger,
    IUnitOfWork uow,
    IMapper mapper) : base((UserStore<User, Role, BlogDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
        optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _users = uow.Set<User>();
        _mapper = mapper;
    }

    public Task<User> FindByEmailOrNameAsync(string emailOrName)
        => _users.SingleOrDefaultAsync(u => u.Email == emailOrName || u.UserName == emailOrName);

    public Task<UserDetailsViewModel> GetUserDetailsForAdminAsync(int userId)
    => _mapper.ProjectTo<UserDetailsViewModel>(_users).SingleOrDefaultAsync(u => u.Id == userId);

    public Task<ChangeUserNameViewModel> GetUserForChangeUserNameAsync(int userId)
    => _mapper.ProjectTo<ChangeUserNameViewModel>(_users).SingleOrDefaultAsync(u => u.Id == userId);

    public Task<EditProfileViewModel> GetUserToEditProfileAsync(int userId)
    => _mapper.ProjectTo<EditProfileViewModel>(_users.Include(u => u.UserInformation)).SingleOrDefaultAsync(u => u.Id == userId);

    public Task<EditUserRolesViewModel> GetUserRolesToEditAsync(int userId) //TODO : add user information in mapping
        => _mapper.ProjectTo<EditUserRolesViewModel>(_users.Include(u=>u.UserRoles).ThenInclude(r=>r.Role)).SingleOrDefaultAsync(u => u.Id == userId);

    public UsersStatisticsViewModel GetUsersStatistics()
    {
        var now = DateTime.Now;
        var thirtyDaysAgo = now.AddDays(-30);
        var zevenDaysAgo = now.AddDays(-7);
        return new UsersStatisticsViewModel()
        {
            UsersCount = _users.Count(),
            ActiveUsersCount = _users.Count(u => u.IsActive),
            ConfirmedEmailUsersCount = _users.Count(u => u.EmailConfirmed),
            ConfirmedUsersCount = _users.Count(u => u.EmailConfirmed && u.IsActive),
            RegistrationLastThirtyDaysCount = _users.Count(u => u.CreatedDateTime > thirtyDaysAgo && u.CreatedDateTime < now),
            RegistrationLastSevenDaysCount= _users.Count(u => u.CreatedDateTime > zevenDaysAgo && u.CreatedDateTime < now),
            ExternalLogins = _users.Count(u => u.UserLogins.Any())
        };
    }

    public SearchingUsersViewModels GetUsersWithFilterAndPagination(UserFiltersAndPaginationViewModel filters, int take = 20)
    {
        var users = _users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filters.UserName))
            users = users.Where(u => u.UserName.Contains(filters.UserName));
        if (!string.IsNullOrWhiteSpace(filters.Email))
            users = users.Where(u => u.Email.Contains(filters.Email));

        switch (filters.ActiveStatus)
        {
            case AllTrueFalseEnum.Yes:
                users = users.Where(u => u.IsActive);
                break;
            case AllTrueFalseEnum.No:
                users = users.Where(u => !u.IsActive);
                break;
        }


        switch (filters.EmailConfirmedStatus)
        {
            case AllTrueFalseEnum.Yes:
                users = users.Where(u => u.EmailConfirmed);
                break;
            case AllTrueFalseEnum.No:
                users = users.Where(u => !u.EmailConfirmed);
                break;
        }

        var allRecordCount = users.Count();
        if(allRecordCount == 0)
        {
            filters.PagesCount = 0;
            filters.CurrentPage = 0;
            return new SearchingUsersViewModels()
            {
                filters = filters,
                Users = new List<UserForAdminViewModel>()
            };
        }

        var allPageCount = (int)Math.Ceiling((decimal)allRecordCount / take);
        if (filters.CurrentPage < 1) filters.CurrentPage = 1;
        if (filters.CurrentPage > allPageCount) filters.CurrentPage = allPageCount;
        var skip = (filters.CurrentPage - 1) * take;
        filters.PagesCount = allPageCount;
        return new SearchingUsersViewModels()
        {
            filters = filters,
            Users = _mapper.ProjectTo<UserForAdminViewModel>(users.Skip(skip).Take(take)).ToList()
        };
    }
}