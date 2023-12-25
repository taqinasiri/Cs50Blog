using Blog.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.DataLayer.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasOne(userClaim => userClaim.User)
            .WithMany(user => user.UserClaims)
            .HasForeignKey(userClaim => userClaim.UserId);

        builder.ToTable("UserClaims");
    }
}
