using Book_Shelf.Interfaces;
using Book_Shelf.Models;
using Book_Shelf.ViewModel;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserDetailsViewModel> GetUserById(int id)
        {

            
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted == false);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                return new UserDetailsViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Role = Enum.Parse<UserRole>(user.Role.ToString(), true) // Convert to UserRole enum

                };
           
        }

        public async Task<bool> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email and password cannot be null or empty");
            }
            // Check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }
            return true;

        }

        public async Task<bool> SignUp(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            // Check if the user already exists
            var existingUser =await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }
            // Add the new user to the database
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
            return true;
        }


    }

    }
