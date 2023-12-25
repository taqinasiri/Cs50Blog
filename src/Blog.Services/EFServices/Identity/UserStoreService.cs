using Blog.Services.Contracts.Identity;
using Blog.DataLayer.Context;
using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blog.Services.EFServices.Identity;

public class UserStoreService : UserStore<User, Role, BlogDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>,
    IUserStoreService
{
    public UserStoreService(IUnitOfWork uow, IdentityErrorDescriber describer = null) : base((BlogDbContext)uow, describer) { }
}