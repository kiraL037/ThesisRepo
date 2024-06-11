using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(string username, string passwordHash, string salt, bool isAdmin);
    }
}