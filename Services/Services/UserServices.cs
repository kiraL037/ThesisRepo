using Core.Interfaces;
using System;
using System.Threading.Tasks;
using Core.Models;
using System.Windows;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityMethods _securityMethods;

        public UserService(IUserRepository userRepository, ISecurityMethods securityMethods)
        {
            _userRepository = userRepository;
            _securityMethods = securityMethods;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                return _securityMethods.VerifyPassword(password, user.PasswordHash, user.Salt);
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string username, string passwordHash, string salt, bool isAdmin)
        {
            if (await _userRepository.UserExistsAsync(username))
            {
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt,
                IsAdmin = isAdmin
            };

            await _userRepository.CreateUserAsync(user);
            return true;
        }
    }
}