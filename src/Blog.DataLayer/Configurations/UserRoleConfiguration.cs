using Blog.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataLayer.Configurations;

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(userRole => userRole.Role).
                WithMany(role => role.UserRoles)
                .HasForeignKey(userRole => userRole.RoleId);

            builder.HasOne(userRole => userRole.User).
                WithMany(user => user.UserRoles)
                .HasForeignKey(userRole => userRole.UserId);

            builder.ToTable("UserRoles");
        }
    }