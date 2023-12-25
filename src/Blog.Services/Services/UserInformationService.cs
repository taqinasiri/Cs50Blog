using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Services;
public class UserInformationService : GenericService<UserInformation>, IUserInformationService
{
    private readonly DbSet<UserInformation> _userInformation;
    public UserInformationService(IUnitOfWork uow, IMapper mapper) : base(uow)
    {
        _userInformation = uow.Set<UserInformation>();
    }

    public Task<UserInformation> GetUserInformationByUserIdAsync(int userId) =>
    _userInformation.SingleOrDefaultAsync(ui => ui.UserId == userId);
}
