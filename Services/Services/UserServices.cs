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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityMethods _securityMethods;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                return _securityMethods.VerifyPassword(password, user.PasswordHash);
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, bool isAdmin)
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
                IsAdmin = isAdmin
            };

            await _userRepository.CreateUserAsync(user);
            return true;
        }
    }
}