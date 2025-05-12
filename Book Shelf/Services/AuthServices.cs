using Book_Shelf.Interfaces;
using Book_Shelf.Models;
using Book_Shelf.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Book_Shelf.Services
{
    public class AuthServices : IAuthService
    {
        private readonly BookShelfContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthServices(BookShelfContext context, IConfiguration config, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _config = config;
            _passwordHasher = passwordHasher;
        }

        /// Forgot Password 
        public bool ForgotPassword(LoginViewModel user)
        {
           var userDetails = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userDetails == null)
            {
                throw new InvalidOperationException("User not found");
            }
            // Here you can implement the logic to send a password reset email
            // For example, using an email service to send a link to reset the password
            return true;
        }


        /// Generates a JWT token for the user. 
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
        /// Gets all users from the database.
        public Task<List<UserDetailsViewModel>> GetAllUsers()
        {
            return _context.Users
                .Where(u => u.IsDeleted == false && u.Role == UserRole.Customer)
                .Select(u => new UserDetailsViewModel
                {
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    Role = (UserRole)u.Role // Convert to UserRole enum
                })
                .ToListAsync();
        }

        /// Gets a user by email from the database.
        public Task<User?> GetUserByEmail(string email)
        {
            return _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        /// Gets a user by ID from the database.
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
                Role = (UserRole)user.Role

            };

        }

        /// Signs in a user with the provided email and password.
        public async Task<bool> SignIn(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email and password cannot be null or empty");
            }
            var user = await GetUserByEmail(email);

            var hashedPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (hashedPassword == PasswordVerificationResult.Failed)
            {
                throw new InvalidOperationException("Invalid email or password");
            }
            // Check if the user is deleted
            if (user.IsDeleted)
            {
                throw new InvalidOperationException("User is deleted");
            }
            // Check if the user is not a customer
            if (user.Role != UserRole.Customer)
            {
                throw new InvalidOperationException("User is not a customer");
            }

            // Check if the user exists
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }
            return true;

        }

        /// Signs up a new user and adds them to the database.
        public async Task<bool> SignUp(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            
            var hashedPassword = _passwordHasher.HashPassword(user, user.Password);
            user.Password = hashedPassword;

            // Check if the user already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
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
