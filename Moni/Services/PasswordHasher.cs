using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Moni.Services.Classes;

namespace Moni.Services
{
    public class PasswordHasher
    {
        /// <summary>
        /// Hashs a password.
        /// </summary>
        /// <param name="password">Password to be hashed</param>
        /// <param name="salt">Preexistent/stored salt, if any</param>
        /// <returns></returns>
        public PasswordHash HashPassword(string password, string salt = null)
        {
            var saltArray = string.IsNullOrWhiteSpace(salt)
                ? GenerateSalt()
                : Convert.FromBase64String(salt).ToArray();

            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                saltArray,
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));

            return new PasswordHash(hashed, salt ?? Convert.ToBase64String(saltArray));
        }

        /// <summary>
        /// Generate a random Salt.
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateSalt()
        {
            var saltArray = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltArray);
            }

            return saltArray;
        }
    }
}
