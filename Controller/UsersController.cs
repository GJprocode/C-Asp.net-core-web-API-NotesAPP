using Microsoft.AspNetCore.Mvc; // Provides API controller functionality.
using NotesBE.Data; // Access to the repository for database interactions.
using NotesBE.Models; // Contains the User model.

namespace NotesBE.Controllers;

[ApiController] // Indicates this class handles API requests.
[Route("api/[controller]")] // Defines the base route for this controller as "api/users".
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository; // Repository for user data access.

    public UsersController(UserRepository userRepository)
    {
        _userRepository = userRepository; // Inject the UserRepository dependency.
    }

    // GET: api/users
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)] // Specifies the response type for Swagger documentation.
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsersAsync(); // Fetch all users from the database.
        return Ok(users); // Return the list of users.
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)] // Success response with a User object.
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Response when the user is not found.
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id); // Fetch user by ID.
        if (user == null) return NotFound("User not found."); // Return 404 if the user doesn't exist.
        return Ok(user); // Return the user data.
    }
}
