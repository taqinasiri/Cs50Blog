namespace Blog.Services.Contracts;
public interface IUserInformationService: IGenericService<UserInformation>
{
    Task<UserInformation> GetUserInformationByUserIdAsync(int userId);
}
