namespace Core.Interfaces
{
    public interface ISecurityMethods
    {
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string hashedPassword);
    }
}