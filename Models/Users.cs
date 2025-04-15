// namespace NotesBE.Models;

// // Represents a user in the system.
// public class User
// {
//     public int Id { get; set; } // Unique identifier for the user.
//     public string Username { get; set; } = string.Empty; // The user's chosen username (required).
//     public string Password { get; set; } = string.Empty; // Plain-text password for input (not stored in the database).
//     public byte[] PasswordHash { get; set; } = Array.Empty<byte>(); // Securely hashed version of the password.
//     public byte[] PasswordSalt { get; set; } = Array.Empty<byte>(); // Salt used for password hashing.
//     public string Email { get; set; } = string.Empty; // The user's email address (used for contact or recovery).
//     public DateTime CreatedAt { get; set; } // Timestamp for when the user was created.
//     public DateTime UpdatedAt { get; set; } // Timestamp for when the user's information was last updated.
// }


using System;

namespace NotesBE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}