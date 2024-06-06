using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt;

namespace Services.Services
{
    public static class SecurityMethods
    {
        public static string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        public static bool VerifyPassword(string password, string storedHash, string salt)
        {
            return BCrypt.Net.BCrypt.Verify(password + salt, storedHash);
        }

        public static string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}