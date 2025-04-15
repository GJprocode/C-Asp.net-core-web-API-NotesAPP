namespace NotesBE.Models
{
    // This class represents the response sent back after a user successfully authenticates.
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!; // The JWT token given to the user after login.
    }
}
