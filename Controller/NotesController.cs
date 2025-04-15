using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesBE.Data;
using NotesBE.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NotesBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _noteRepository;

        public NotesController(NoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotes([FromQuery] string? titleFilter = null)
        {
            Console.WriteLine("GetNotes endpoint hit");
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                Console.WriteLine("userId claim is null");
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);
            Console.WriteLine($"userid: {userId}");
            var notes = await _noteRepository.GetNotesByUserIdAsync(userId);

            if (!string.IsNullOrEmpty(titleFilter))
            {
                notes = notes.Where(n => n.Title.Contains(titleFilter, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(notes);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNote([FromBody] Note note)
        {
            Console.WriteLine("CreateNote endpoint hit");
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                Console.WriteLine("userId claim is null");
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);
            Console.WriteLine($"userid: {userId}");
            note.UserId = userId;
            note.CreatedAt = note.UpdatedAt = DateTime.UtcNow;

            var id = await _noteRepository.CreateNoteAsync(note);
            note.Id = id;

            return CreatedAtAction(nameof(GetNotes), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] Note note)
        {
            if (id != note.Id) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            if (existingNote == null || existingNote.UserId != userId) return NotFound();

            note.UpdatedAt = DateTime.UtcNow;
            await _noteRepository.UpdateNoteAsync(note);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            if (existingNote == null || existingNote.UserId != userId) return NotFound();

            await _noteRepository.DeleteNoteAsync(id, userId);
            return NoContent();
        }
    }
}