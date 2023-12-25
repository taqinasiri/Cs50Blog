using Blog.Common.Mvc;
using Blog.DataLayer.Context;
using Blog.Entities.Identity;
using Blog.Services;
using Blog.Services.Contracts;
using Blog.Services.Contracts.Identity;
using Blog.Services.EFServices;
using Blog.Services.EFServices.Identity;
using Blog.ViewModels.Application;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Blog.IocConfig;

public static class AddCustomServicesExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var connectionStrings = provider.GetRequiredService<IOptionsMonitor<ConnectionStringsModel>>().CurrentValue;
        services.AddDbContext<BlogDbContext>(options =>
        {
            options.UseSqlServer(connectionStrings.BlogDbContextConnection);
        });
        #region Register Identity Sevices
        services.AddScoped<IRoleManagerService, RoleManagerService>();
        services.AddScoped<RoleManager<Role>, RoleManagerService>(); //this ioc for identity base

        services.AddScoped<IRoleStoreService, RoleStoreService>();
        services.AddScoped<RoleStore<Role, BlogDbContext, int, UserRole, RoleClaim>, RoleStoreService>();

        services.AddScoped<IUserManagerService, UserManagerService>();
        services.AddScoped<UserManager<User>, UserManagerService>();

        services.AddScoped<IUserStoreService, UserStoreService>();
        services.AddScoped<UserStore<User, Role, BlogDbContext, int, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>, UserStoreService>();

        services.AddScoped<ISignInManagerService, SignInManagerService>();
        services.AddScoped<SignInManager<User>, SignInManagerService>();

        services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimService>();
        services.AddScoped<UserClaimsPrincipalFactory<User, Role>, UserClaimService>();


        #endregion

        services.AddScoped<IUnitOfWork, BlogDbContext>();
        services.AddScoped<IEmailSenderService, EmailSenderService>();
        services.AddScoped<IPostService,PostService>();

        services.AddIdentity<User, Role>(identityOptions =>
            {
                SetPasswordOptions(identityOptions.Password);

                identityOptions.Lockout.AllowedForNewUsers = false;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                identityOptions.Lockout.MaxFailedAccessAttempts = 3;

                identityOptions.SignIn.RequireConfirmedEmail = true;
                identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
                identityOptions.SignIn.RequireConfirmedAccount = true;

                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            })
            .AddUserStore<UserStoreService>()
            .AddUserManager<UserManagerService>()
            .AddRoleStore<RoleStoreService>()
            .AddRoleManager<RoleManagerService>()
            .AddSignInManager<SignInManagerService>()
            .AddDefaultTokenProviders();


        services.Configure<SecurityStampValidatorOptions>(options =>
        {
            options.ValidationInterval = TimeSpan.FromMinutes(5);
        });

        services.AddRazorViewRenderer();
        return services;
    }


    private static IServiceCollection AddRazorViewRenderer(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IViewRendererService, ViewRendererService>();
        return services;
    }

    private static void SetPasswordOptions(PasswordOptions passwordOptions)
    {
        passwordOptions.RequireDigit = false;
        passwordOptions.RequireLowercase = false;
        passwordOptions.RequireUppercase = false;
        passwordOptions.RequireNonAlphanumeric = false;
        passwordOptions.RequiredUniqueChars = 0;
    }

}