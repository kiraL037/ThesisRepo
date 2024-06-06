using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IUserService
    {
        Task<bool> AuthenticateUserAsync(string username, string password);
        Task RegisterUserAsync(string username, string password, bool isAdmin);
    }
}