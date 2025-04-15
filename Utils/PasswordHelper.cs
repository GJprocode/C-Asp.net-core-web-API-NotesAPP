// // / NotesBE.Utils/PasswordHelper.cs
// using System.Security.Cryptography;

// namespace NotesBE.Utils
// {
//     public static class PasswordHelper
//     {
//         public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//         {
//             using (var hmac = new HMACSHA512())
//             {
//                 passwordSalt = hmac.Key;
//                 passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//             }
//         }

//         public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
//         {
//             using (var hmac = new HMACSHA512(storedSalt))
//             {
//                 var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                 return computedHash.SequenceEqual(storedHash);
//             }
//         }
//     }
// }
using System.Security.Cryptography; // Provides cryptographic services for secure password hashing

namespace NotesBE.Utils
{
    public static class PasswordHelper
    {
        // Generates a secure password hash along with a unique salt
        // These are stored in the database for verifying user authentication
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Use HMACSHA512 to generate a salt and hash for the given password
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key; // The salt is the cryptographic key
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Compute the hash
            }
        }

        // Verifies if the provided password matches the stored hash and salt
        // This is used during login to authenticate the user
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            // Recompute the hash using the provided salt and compare it to the stored hash
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash); // True if the hashes match
            }
        }
    }
}

/*
** Summary:
** - This file contains utilities for secure password management.
** - `CreatePasswordHash`: Generates a hashed password and a unique salt for secure storage.
** - `VerifyPasswordHash`: Validates a given password against a stored hash and salt.
** - Ensures robust authentication by using cryptographic hashing with HMACSHA512.
*/