using Blog.DataLayer.Context;
using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Services.Contracts.Identity;

namespace Blog.Services.EFServices.Identity;

public class RoleStoreService : RoleStore<Role, BlogDbContext, int, UserRole, RoleClaim>, IRoleStoreService
{
    public RoleStoreService(IUnitOfWork uow, IdentityErrorDescriber describer = null) : base((BlogDbContext)uow, describer) { }
}

