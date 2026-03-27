using System.Text.RegularExpressions;
using BCrypt.Net;
using MediCore.Api.Utilities;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            try
            {
                // Validate Password Match
                if (model.NewPassword != model.ConfirmPassword)
                    throw new Exception(ErrorMessages.PasswordsDoNotMatch);

                // Validate Password Strength
                if (!IsValidPassword(model.NewPassword))
                    throw new Exception(ErrorMessages.InvalidPassword);

                // Check User
                var user = await _userRepository.GetByEmailAsync(model.Email);

                if (user == null)
                    throw new Exception(ErrorMessages.UserNotFound);

                // Hash Password using BCrypt
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                // Update DB
                await _userRepository.UpdateAsync(user);

                throw new Exception(ErrorMessages.PasswordUpdatedSuccess);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        // Password Validation Method
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            var hasUpper = Regex.IsMatch(password, "[A-Z]");
            var hasNumber = Regex.IsMatch(password, "[0-9]");

            return hasUpper && hasNumber;
        }
    }
}