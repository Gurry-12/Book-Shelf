using Book_Shelf.Models;
using Book_Shelf.ViewModel;

namespace Book_Shelf.Interfaces
{
    public interface IAuthService
    {
        Task<bool> SignUp(User user);

        Task<bool> SignIn(string email, string password);
         Task<UserDetailsViewModel> GetUserById(int id);
        Task<List<UserDetailsViewModel>> GetAllUsers();
        string GenerateJwtToken(User user);
        Task<User> GetUserByEmail(string email);

        bool ForgotPassword(LoginViewModel user);
    }
}
