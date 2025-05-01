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
            catch (InvalidOperationException ex) {

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
               
                    return Ok("User signed in successfully");

               
            }
            catch(InvalidOperationException ex)
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



    }
}
