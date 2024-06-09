using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISecurityMethods
    {
        string HashPassword(string password, out string salt);
        bool VerifyPassword(string password, string hashedPassword);
    }
}