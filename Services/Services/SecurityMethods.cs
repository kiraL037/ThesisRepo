using Core.Interfaces;

namespace Services.Services
{
    public class SecurityMethods : ISecurityMethods
    {
        public string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        public bool VerifyPassword(string password, string storedHash, string salt)
        {
            return BCrypt.Net.BCrypt.Verify(password + salt, storedHash);
        }

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}