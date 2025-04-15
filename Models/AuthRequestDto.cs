namespace NotesBE.Models
{
    // Data Transfer Object (DTO) for authentication requests.
    public class AuthRequestDto
    {
        public string Username { get; set; } = null!; // Username provided by the user.
        public string Password { get; set; } = null!; // Password provided by the user.
    }
}
