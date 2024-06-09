using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task CreateUserAsync(string username, string hashedPassword, string salt, bool isAdmin);
        Task<bool> UserExistsAsync(string username);
    }
}
