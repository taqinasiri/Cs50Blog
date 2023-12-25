using Blog.Entities;
using Blog.Entities.Identity;
using Blog.ViewModels.Account;
using Blog.ViewModels.Dashboard;
using Blog.ViewModels.Roles;
using Blog.ViewModels.Users;

namespace Blog.Web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterViewModel, User>();
        CreateMap<User, ChangeUserNameViewModel>();
        CreateMap<User, EditProfileViewModel>()
            .ForMember(dest => dest.Avatar, opt => opt.Ignore())
            .ForMember(dest => dest.Biography, options => options.MapFrom(src => src.UserInformation.Biography))
            .ForMember(dest => dest.Website, options => options.MapFrom(src => src.UserInformation.Website))
            .ForMember(dest => dest.Linkedin, options => options.MapFrom(src => src.UserInformation.Linkedin))
            .ForMember(dest => dest.Instagram, options => options.MapFrom(src => src.UserInformation.Instagram))
            .ForMember(dest => dest.Twitter, options => options.MapFrom(src => src.UserInformation.Twitter));
        CreateMap<User, UserForAdminViewModel>();
        CreateMap<User, UserDetailsViewModel>()
            .ForMember(dest => dest.ExternalLogins, options => options.MapFrom(src => src.UserLogins.Select(ul => ul.ProviderDisplayName).ToList()));
        CreateMap<Role, RoleForAdminViewModel>()
            .ForMember(dest => dest.UsersInRoleCount, options => options.MapFrom(src => src.UserRoles.Count()));
        CreateMap<Role, EditRoleViewModel>()
            .ForMember(dest => dest.Permissions, options => options.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission).ToList()));
        CreateMap<AddRoleViewModel, Role>()
            .ForMember(dest => dest.RolePermissions, options => options.MapFrom(src => src.Permissions.Select(rp => new RolePermission()
            {
                Permission = rp,
            })));
        CreateMap<Role, RoleDetailsViewModel>()
          .ForMember(dest => dest.Permissions, options => options.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission).ToList()))
          .ForMember(dest => dest.UsersInRoleCount, options => options.MapFrom(src => src.UserRoles.Count()));
        CreateMap<User,EditUserRolesViewModel>()
            .ForMember(dest => dest.Roles, options => options.MapFrom(src => src.UserRoles.Select(r => r.Role.Name)))
            .ForMember(dest => dest.Biography, options => options.MapFrom(src => src.UserInformation.Biography))
            .ForMember(dest => dest.Website, options => options.MapFrom(src => src.UserInformation.Website))
            .ForMember(dest => dest.Linkedin, options => options.MapFrom(src => src.UserInformation.Linkedin))
            .ForMember(dest => dest.Instagram, options => options.MapFrom(src => src.UserInformation.Instagram))
            .ForMember(dest => dest.Twitter, options => options.MapFrom(src => src.UserInformation.Twitter));
        CreateMap<EditUserRolesViewModel, UserInformation>();
    }
}
