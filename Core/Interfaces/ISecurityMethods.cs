namespace Core.Interfaces
{
    public interface ISecurityMethods
    {
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string hashedPassword, string salt);
        string GenerateSalt();
    }
}