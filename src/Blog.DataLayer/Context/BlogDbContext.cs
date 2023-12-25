using Blog.DataLayer.Context;
using Blog.Entities;
using Blog.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataLayer.Context
{
    public class BlogDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IUnitOfWork
    {
        public BlogDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public void MarkAsDeleted<TEntity>(TEntity entity) => base.Entry(entity).State = EntityState.Deleted;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Seed Admin Role

            builder.Entity<Role>().HasData(new Role
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = 1,
                ConcurrencyStamp = "1"
            });

            var user = new User()
            {
                Id = 1,
                Email = "admin@blog.com",
                NormalizedEmail = "ADMIN@BLOG.COM",
                EmailConfirmed = true,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                IsActive = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin123");
            builder.Entity<User>().HasData(user);

            builder.Entity<UserRole>().HasData(
                new UserRole()
                {
                    RoleId = 1,
                    UserId = 1
                });
            #endregion
            builder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }
    }
}