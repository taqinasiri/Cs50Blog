using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Services.Services.Identity;
public class UserStoreService : UserStore<User, Role, BlogDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>,
    IUserStoreService
{
    public UserStoreService(IUnitOfWork uow, IdentityErrorDescriber describer = null) : base((BlogDbContext)uow, describer) { }
}
