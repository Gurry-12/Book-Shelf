using Book_Shelf.Interfaces;
using Book_Shelf.Models;
using Book_Shelf.ViewModel;
using System.Linq;

namespace Book_Shelf.Services
{
    public class AuthServices : IAuthService
    {
        private readonly BookShelfContext _context;

        public AuthServices(BookShelfContext context)
        {
            _context = context;
        }

        public UserDetailsViewModel GetUserById(int id)
        {
            
            return _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted) != null ? new UserDetailsViewModel
            {
                
                Name = _context.Users.FirstOrDefault(u => u.Id == id).Name,
                Email = _context.Users.FirstOrDefault(u => u.Id == id).Email,
                Phone = _context.Users.FirstOrDefault(u => u.Id == id).Phone,
                Address = _context.Users.FirstOrDefault(u => u.Id == id).Address,
                Role = _context.Users.FirstOrDefault(u => u.Id == id).Role
            } : null;
        }

        public bool SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email and password cannot be null or empty");
            }
            // Check if the user exists
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }
            return true;

        }

        public bool SignUp(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            // Check if the user already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            // Add the new user to the database
            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }


    }

    }
