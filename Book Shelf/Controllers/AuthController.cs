using Book_Shelf.Interfaces;
using Book_Shelf.Models;
using Book_Shelf.ViewModel;
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
        public IActionResult SignUp([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            var result = _authService.SignUp(user);
            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("User registration failed");
            }
        }

        [HttpPost("SignIn")]
        public IActionResult SignIn([FromBody] LoginViewModel login)
        {
            if (login == null)
            {
                return BadRequest("Login model cannot be null");
            }
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and password cannot be null or empty");
            }
            var result = _authService.SignIn(login.Email, login.Password);
            if (result)
            {
                return Ok("User signed in successfully");
            }
            else
            {
                return Unauthorized("Invalid email or password");
            }
        }

        [HttpGet("GetUser/{id}")]
        public IActionResult GetUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid user ID");
            }
            var user = _authService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

    }
}
