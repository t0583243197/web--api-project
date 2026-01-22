using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Models.DTO;
using WebApplication2.BLL;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserBll _userBll;

        public AccountController(IUserBll userBll)
        {
            _userBll = userBll;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _userBll.ValidateUser(login.Email, login.Password);

            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere1234567890!");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            Console.WriteLine("=== CONTROLLER REGISTER CALLED ===");
            Console.WriteLine($"UserDto received: {userDto?.Email}");
            Console.WriteLine($"UserBll is null: {_userBll == null}");
            
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid:");
                    foreach (var error in ModelState)
                    {
                        Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                    return BadRequest(ModelState);
                }
                    
                Console.WriteLine("About to call _userBll.AddUser");
                await _userBll.AddUser(userDto);
                Console.WriteLine("_userBll.AddUser completed");
                
                return Ok(new { message = "User registered successfully. You can now log in." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}