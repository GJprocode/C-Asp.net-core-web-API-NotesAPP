using Dapper;
using System.Data;
using NotesBE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesBE.Data
{
    public class NoteRepository
    {
        private readonly IDbConnection _dbConnection;

        public NoteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(int userId)
        {
            string query = "SELECT * FROM NotesAPP.Notes WHERE UserId = @UserId ORDER BY CreatedAt DESC";
            return await _dbConnection.QueryAsync<Note>(query, new { UserId = userId });
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            string query = "SELECT * FROM NotesAPP.Notes WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Note>(query, new { Id = id });
        }

        public async Task<int> CreateNoteAsync(Note note)
        {
            string query = @"
                INSERT INTO NotesAPP.Notes (UserId, Title, Content, CreatedAt, UpdatedAt)
                VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";
            return await _dbConnection.ExecuteScalarAsync<int>(query, note);
        }

        public async Task<bool> UpdateNoteAsync(Note note)
        {
            string query = @"
                UPDATE NotesAPP.Notes
                SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
                WHERE Id = @Id AND UserId = @UserId;
            ";
            int rowsAffected = await _dbConnection.ExecuteAsync(query, note);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            string query = "DELETE FROM NotesAPP.Notes WHERE Id = @Id AND UserId = @UserId";
            int rowsAffected = await _dbConnection.ExecuteAsync(query, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
    }
}