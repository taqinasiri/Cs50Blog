using Blog.DataLayer.Context;
using Blog.Entities.Identity;
using Blog.Services.Contracts.Identity;
using Blog.ViewModels.Account;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.Services.EFServices.Identity;

public class UserManagerService : UserManager<User>, IUserManagerService
{
    private readonly DbSet<User> _users;
    public UserManagerService(IUserStoreService store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher,
        IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManagerService> logger,
        IUnitOfWork uow) : base((UserStore<User, Role, BlogDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>)store,
            optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _users = uow.Set<User>();
    }

    public Task<List<ShowUserViewModel>> UsersPreview() => _users
        .Select(u => new ShowUserViewModel()
        {
            Id = u.Id,
            CreatedDateTime = u.CreatedDateTime,
            IsActive = u.IsActive,
            UserName = u.UserName

        }).ToListAsync();

}
