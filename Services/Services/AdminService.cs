using Core.Interfaces;
using System;
using System.Threading.Tasks;
using Core.Models;

namespace Services.Services
{
    public class AdminService 
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityMethods _securityMethods;

        public AdminService(IUserRepository userRepository, ISecurityMethods securityMethods)
        {
            _userRepository = userRepository;
            _securityMethods = securityMethods;
        }

        public async Task CreateAdminAsync(string username, string password)
        {
            if (await _userRepository.UserExistsAsync(username))
            {
                throw new Exception("User already exists.");
            }

            string salt = _securityMethods.GenerateSalt();
            var hashedPassword = _securityMethods.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Salt = salt,
                IsAdmin = true
            };

            await _userRepository.CreateUserAsync(user);
        }
    }
}