// NotesBE.Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using NotesBE.Data;
using NotesBE.Models;
using NotesBE.Utils;
using System;
using System.Threading.Tasks;

namespace NotesBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly string _jwtSecret;

        public AuthController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtSecret = configuration["JWT_SECRET"]
                ?? throw new InvalidOperationException("JWT_SECRET is not configured.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                return BadRequest(new { Error = "Password is required." });

            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUserByUsername != null)
                return Conflict(new { Error = "Username already exists." });

            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUserByEmail != null)
                return Conflict(new { Error = "Email already exists." });

            PasswordHelper.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedAt = user.UpdatedAt = DateTime.UtcNow;

            var userId = await _userRepository.CreateUserAsync(user);
            return Ok(new { Message = "User registered successfully", UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser == null || existingUser.PasswordHash == null || existingUser.PasswordSalt == null)
                return Unauthorized("Invalid username or password.");

            if (!PasswordHelper.VerifyPasswordHash(user.Password, existingUser.PasswordHash, existingUser.PasswordSalt))
                return Unauthorized("Invalid username or password.");

            try
            {
                var token = JwtHelper.GenerateToken(existingUser.Username, existingUser.Id, _jwtSecret);
                Console.WriteLine($"JWT Token generated for user {existingUser.Username}: {token}");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating token: {ex.Message}");
                return StatusCode(500, "An error occurred while generating the token.");
            }
        }
    }
}