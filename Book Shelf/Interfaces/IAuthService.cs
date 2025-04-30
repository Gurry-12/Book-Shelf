using Book_Shelf.Models;
using Book_Shelf.ViewModel;

namespace Book_Shelf.Interfaces
{
    public interface IAuthService
    {
        bool SignUp(User user);

        bool SignIn(string email, string password);
        UserDetailsViewModel GetUserById(int id);
    }
}
