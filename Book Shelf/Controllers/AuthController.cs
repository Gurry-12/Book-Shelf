using Book_Shelf.Interfaces;
using Book_Shelf.Models;
using Book_Shelf.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Book_Shelf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/<AuthController>
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            try
            {
                var result = await _authService.SignUp(user);


                return Ok("User registered successfully");
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong on the server");
            }

        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] LoginViewModel login)
        {
            if (login == null)
            {
                return BadRequest("Login model cannot be null");
            }
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and password cannot be null or empty");
            }
            try
            {
                var result = await _authService.SignIn(login.Email, login.Password);
                if (result)
                {
                    // Check if the user is deleted
                    var user = await _authService.GetUserByEmail(login.Email);
                    // Generate JWT token
                    var token = _authService.GenerateJwtToken(user);

                   

                    // Return the token in the response
                    return Ok(new { Message = "User signed in successfully" , token});

                }
                else
                {
                    return Unauthorized("Invalid email or password");
                }



            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            try
            {
                var user = await _authService.GetUserById(id);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message); // 404 if user not found
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong on the server");
            }
        }

        /// Gets all users from the database.
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong on the server");
            }
        }

        [Authorize(Roles = "Admin,Customer")]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginViewModel user)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("Email cannot be null or empty");
            }
            try
            {
               bool valid =  _authService.ForgotPassword(user);
                // Implement your password reset logic here
                // For example, send a password reset email
                return Ok("Password reset link sent to your email");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong on the server");
            }
        }


        // Note: JWT tokens are stateless, so logging out usually involves removing the token from the client side.
        // If you are using cookies, you can clear the cookie here.
        // If you are using JWT tokens, you can just inform the client to remove the token.

        //[Authorize]
        //[HttpGet("Logout")]
        //public IActionResult Logout()
        //{
        //    try
        //    {                // Check if the user is authenticated
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            // Clear the JWT cookie
        //            return Ok("User logged out successfully");
        //        }

        //        return Unauthorized("User is not authenticated");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Something went wrong on the server");
        //    }
        //}



    }
}
