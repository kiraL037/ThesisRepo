using Core.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Services.Services
{
    public class AdminService 
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityMethods _securityMethods;

        public AdminService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAdminAsync(string username, string password)
        {
            if (await _userRepository.UserExistsAsync(username))
            {
                throw new Exception("User already exists.");
            }

            var hashedPassword = _securityMethods.HashPassword(password, out string salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                IsAdmin = true
            };

            await _userRepository.CreateUserAsync(user);
        }
    }
}