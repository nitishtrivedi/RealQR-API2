namespace RealQR_API.Services
{
    public interface IUserService
    {
        Task<string> Register(string username, string firstname, string lastname, string password, string email, bool isUserAdmin);
        Task<string> Login(string username, string password);
        //string GetUsername();
    }
}
