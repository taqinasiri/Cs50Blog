using Blog.Services.Contracts.Identity;
using DNTCommon.Web.Core;

namespace Blog.IocConfig;
public static class DbContextOptionsExtensions
{
    public static void InitializeDb(this IServiceProvider serviceProvider)
    => serviceProvider.RunScopedService<IIdentityDbInitializer>(identityDbInitialize =>
        {
            identityDbInitialize.Initialize();
            identityDbInitialize.SeedData();
        });
}
