using DotNetEnv; // Ensure you have this import to load the .env values
using Microsoft.IdentityModel.Tokens; // Handles token security and signing
using System; // Provides fundamental classes like DateTime and Exception
using System.IdentityModel.Tokens.Jwt; // Handles JSON Web Token (JWT) creation and validation
using System.Security.Claims; // Manages user-specific data (claims) for authentication
using System.Text; // Provides utilities for handling text and encoding

namespace NotesBE.Utils
{
    public static class JwtHelper
    {
        // Method to generate a JWT token for a user
        public static string GenerateToken(string username, int userId, string secretKey)
        {
            // Ensure the secret key has no extra spaces and is secure
            secretKey = secretKey.Trim();

            // Check that the secret key is at least 16 characters long for security
            if (secretKey.Length < 16)
            {
                throw new ArgumentException("JWT secret key must be at least 16 characters long.");
            }

            // Convert the secret key into a format usable for signing
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Create signing credentials with the security key
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Load JWT configuration from .env file (instead of hardcoding)
            string jwtIssuer = Env.GetString("JWT_ISSUER") ?? "NotesBE"; // Use the value from .env or fallback to "NotesBE"
            string jwtAudience = Env.GetString("JWT_AUDIENCE") ?? "NotesBEUsers"; // Use the value from .env or fallback to "NotesBEUsers"

            // Define user-related claims that the token will include
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username), // Subject claim (the username)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim("userId", userId.ToString()), // Custom claim for user ID
                new Claim(JwtRegisteredClaimNames.Aud, jwtAudience) // Audience claim (from .env)
            };

            // Create the JWT token, adding claims, issuer, and expiration
            var token = new JwtSecurityToken(
                issuer: jwtIssuer, // Issuer from .env
                audience: jwtAudience, // Audience from .env
                claims: claims, // User-specific data included in the token
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                signingCredentials: credentials // Ensures token authenticity
            );

            // Serialize the token into a string to send back to the user
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
