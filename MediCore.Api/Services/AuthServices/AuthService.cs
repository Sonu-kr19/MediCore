using System.Text.RegularExpressions;
using BCrypt.Net;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;

namespace MediCore.Api.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
    
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    
        public async Task<(bool Success, string Message)> ForgotPasswordAsync(ForgotPasswordDto model)
        {
            try
            {
                //  Validate Password Match
                if (model.NewPassword != model.ConfirmPassword)
                    return (false, "Passwords do not match");
    
                // Validate Password Strength
                if (!IsValidPassword(model.NewPassword))
                    return (false, "Password must be at least 8 characters, include one uppercase letter and one number");
    
                //  Check User
                var user = await _userRepository.GetByEmailAsync(model.Email);
    
                if (user == null)
                    return (false, "User not found");
    
                //  Hash Password using BCrypt
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
    
                //  Update DB
                await _userRepository.UpdateAsync(user);
    
                return (true, "Password updated successfully");
            }
            catch (Exception)
            {
                return (false, "Something went wrong. Please try again.");
            }
        }
    
        //Password Validation Method
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


 