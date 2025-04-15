// using Dapper;
// using System.Data;
// using NotesBE.Models;
// using System.Diagnostics; // Required for Debug.WriteLine

// namespace NotesBE.Data
// {
//     public class UserRepository
//     {
//         private readonly IDbConnection _dbConnection;

//         public UserRepository(IDbConnection dbConnection)
//         {
//             _dbConnection = dbConnection;
//         }

//         public async Task<IEnumerable<User>> GetAllUsersAsync()
//         {
//             string query = "SELECT * FROM NotesAPP.Users";
//             return await _dbConnection.QueryAsync<User>(query);
//         }

//         public async Task<User?> GetUserByIdAsync(int id)
//         {
//             string query = "SELECT * FROM NotesAPP.Users WHERE Id = @Id";
//             return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
//         }

//         public async Task<User?> GetUserByUsernameAsync(string username)
//         {
//             string query = "SELECT * FROM NotesAPP.Users WHERE Username = @Username";
//             return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
//         }

//         public async Task<User?> GetUserByEmailAsync(string email)
//         {
//             string query = "SELECT * FROM NotesAPP.Users WHERE Email = @Email";
//             return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
//         }

//         public async Task<int> CreateUserAsync(User user)
//         {
//             string query = @"
//                 INSERT INTO NotesAPP.Users (Username, PasswordHash, PasswordSalt, Email, CreatedAt, UpdatedAt)
//                 VALUES (@Username, @PasswordHash, @PasswordSalt, @Email, @CreatedAt, @UpdatedAt);
//                 SELECT CAST(SCOPE_IDENTITY() as int);
//             ";

//             // Breakpoint here
//             // Output the query to the Debug output.
//             // Debug.WriteLine($"SQL Query: {query}"); 

//             return await _dbConnection.ExecuteScalarAsync<int>(query, user);
//         }
//     }
// }

/// NotesBE.Data/UserRepository.cs
using Dapper;
using System.Data;
using NotesBE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesBE.Data
{
    public class UserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            string query = "SELECT * FROM NotesAPP.Users";
            return await _dbConnection.QueryAsync<User>(query);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            string query = "SELECT * FROM NotesAPP.Users WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            string query = "SELECT * FROM NotesAPP.Users WHERE Username = @Username";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            string query = "SELECT * FROM NotesAPP.Users WHERE Email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User user)
        {
            string query = @"
                INSERT INTO NotesAPP.Users (Username, PasswordHash, PasswordSalt, Email, CreatedAt, UpdatedAt)
                VALUES (@Username, @PasswordHash, @PasswordSalt, @Email, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";
            return await _dbConnection.ExecuteScalarAsync<int>(query, user);
        }
    }
}