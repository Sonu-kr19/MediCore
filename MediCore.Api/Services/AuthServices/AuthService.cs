using System;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using MediCore.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using MediCore.Api.DTOs.TokenDtos;
using MediCore.Api.Repositories.TokenRepo;
using MediCore.Api.Utilities;
using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Repositories.AuditRepo;

namespace MediCore.Api.Services.AuthServices;

/// <summary>
/// API SERVICE SUMMARY:
/// Handles User Authentication, JWT Generation, and Refresh Token Lifecycle.
/// Security: Uses BCrypt for password verification and HMAC-SHA256 for JWT signing.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _tokenRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    public AuthService(IUserRepository userRepository, IConfiguration configuration, ITokenRepository tokenRepository, IAuditLogRepository auditLogRepository)
    {
        _userRepository=userRepository;
        _configuration=configuration;
        _tokenRepository=tokenRepository;
        _auditLogRepository=auditLogRepository;
    }

    /// <summary>
    /// API ENDPOINT LOGIC: LOGIN
    /// 1. Validates user existence by Email.
    /// 2. Verifies Password using BCrypt.
    /// 3. Returns null on failure (triggers 401 Unauthorized in Controller).
    /// 4. Generates and stores a new Refresh Token in the database.
    /// </summary>
    /// <param name="dto">Contains Email and Password.</param>
    /// <returns>TokenResponseDto (Access + Refresh Tokens) or Null.</returns>
    public async Task<TokenResponseDto> ValidateUserAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (user==null)
        {
            await _auditLogRepository.LogAsync(null, "LOGIN_FAILED", $"Email: {dto.Email} — User not found");
            throw new Exception(ErrorMessages.UserNotFound);
        }
        if (!user.Status)
        {
            await _auditLogRepository.LogAsync(user.UserID, "LOGIN_FAILED", $"Email: {dto.Email} — Account inactive");
            throw new Exception(ErrorMessages.InactiveUser);
        }
        bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!passwordValid)
        {   
            await _auditLogRepository.LogAsync(user.UserID, "LOGIN_FAILED", $"Email: {dto.Email} — Wrong password");
            throw new Exception(ErrorMessages.InvalidCredentials);
        }
        var accessToken = GenerateToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token=refreshToken,
            ExpiryDate=DateTime.UtcNow.AddDays(7),
            IsRevoked=false,
            UserId=user.UserID
        };
        await _tokenRepository.AddRefreshTokenAsync(refreshTokenEntity);
        await _auditLogRepository.LogAsync(user.UserID, "LOGIN_SUCCESS", $"Email: {dto.Email}");
        return new TokenResponseDto
        {
            AccessToken=accessToken,
            RefreshToken=refreshToken
        };
    }

    /// <summary>
    /// API ENDPOINT LOGIC: REFRESH TOKEN
    /// 1. Checks if the token exists, is not revoked, and is not expired.
    /// 2. If valid, Revokes the old token (IsRevoked = true).
    /// 3. Issues a brand new Access Token and a new Refresh Token.
    /// </summary>
    /// <param name="refreshToken">The existing refresh token string.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown if token is invalid or expired.</exception>
    /// <returns>New TokenResponseDto pair.</returns>
    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _tokenRepository.GetRefreshTokenAsync(refreshToken);
        if(storedToken==null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            await _auditLogRepository.LogAsync(null, "TOKEN_REFRESH_FAILED", "Invalid or expired refresh token");
            throw new Exception(ErrorMessages.InvalidRefreshToken);
        }
        var newAccessToken = GenerateToken(storedToken.User);
        storedToken.IsRevoked=true;
        var newRefreshToken=GenerateRefreshToken();
        await _tokenRepository.AddRefreshTokenAsync(new RefreshToken
        {
            Token=newRefreshToken,
            ExpiryDate=DateTime.UtcNow.AddDays(7),
            UserId=storedToken.UserId
        });

        await _auditLogRepository.LogAsync(storedToken.UserId, "TOKEN_REFRESHED", "Access token reissued");

        return new TokenResponseDto
        {
            AccessToken=newAccessToken,
            RefreshToken=newRefreshToken
        };
    }


     /*
     * API ENDPOINT LOGIC: FORGOT PASSWORD
     * 1. Validates that NewPassword and ConfirmPassword match.
     * 2. Validates password strength (min 8 chars, uppercase, lowercase, digit, special char).
     * 3. Checks if user exists by Email.
     * 4. Hashes the new password using BCrypt and updates the database.
     * 5. Returns success message or throws exceptions for any validation failures.
     */
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        try
        {
            //Validate Password if email is Empty
            if (string.IsNullOrEmpty(dto.Email))
                throw new Exception(ErrorMessages.EmailRequired);

            // Validate Password Match
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception(ErrorMessages.PasswordsDoNotMatch);

            // Validate Password Strength
            if (!IsValidPassword(dto.NewPassword))
            {
                await _auditLogRepository.LogAsync(null, "PASSWORD_RESET_FAILED", $"Email: {dto.Email} — Invalid password format");
                throw new Exception(ErrorMessages.InvalidPassword);
            }
            // Check User
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (user == null){
                await _auditLogRepository.LogAsync(null, "PASSWORD_RESET_FAILED", $"Email: {dto.Email} — User not found");
                throw new Exception(ErrorMessages.UserNotFound);
            }
    
            // Hash Password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // Update DB
            await _userRepository.UpdatePasswordAsync(user);
            await _auditLogRepository.LogAsync(user.UserID, "PASSWORD_RESET_SUCCESS", $"Email: {dto.Email} — Password updated");
            return new OkObjectResult(ErrorMessages.PasswordUpdatedSuccess);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
        
    // Password Validation Method
    // <summary>
    // Password must be at least 8 characters
    // Must contain all four character types:
    // At least one uppercase letter (A-Z)
    // At least one lowercase letter (a-z)
    // At least one digit (0-9)
    // At least one special character from: !@#$%^&*(),.?"':{}|
    // <summary>
    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        var hasUpper = Regex.IsMatch(password, "[A-Z]");
        var hasLower = Regex.IsMatch(password, "[a-z]");
        var hasNumber = Regex.IsMatch(password, "[0-9]");
        var hasSpecial = Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]");

        return hasUpper && hasNumber && hasLower && hasSpecial;
    }

    /// <summary>
    /// INTERNAL LOGIC: Generates a cryptographically strong 64-byte random string.
    /// Used for long-lived sessions (7 days) stored in the database.
    /// </summary>
    /// <returns>Base64 encoded string.</returns>
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// INTERNAL LOGIC: Creates a JWT Access Token.
    /// Claims: NameIdentifier (UserID), Email, and Role.
    /// Expiration: 2 Hours.
    /// </summary>
    /// <param name="user">The authenticated user entity.</param>
    /// <returns>Encoded JWT string.</returns>
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role,user.RoleName.ToString())
        };

        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
