using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task CreateUserAsync(string username, string hashedPassword, string salt, bool isAdmin);
        Task<bool> UserExistsAsync(string username);
    }
}
