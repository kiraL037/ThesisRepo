using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.Services.Services
{
    public class AdminService 
    {
        private readonly IUserRepository _userRepository;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAdminAsync(string username, string password)
        {
            string salt = SecurityMethods.GenerateSalt();
            string hashedPassword = SecurityMethods.HashPassword(password, salt);

            await _userRepository.CreateUserAsync(username, hashedPassword, salt, true);
        }
    }
}