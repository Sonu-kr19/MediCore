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
    public AuthService(IUserRepository userRepository, IConfiguration configuration, ITokenRepository tokenRepository)
    {
        _userRepository=userRepository;
        _configuration=configuration;
        _tokenRepository=tokenRepository;
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
            throw new Exception(ErrorMessages.UserNotFound);
        }
        if (!user.Status)
        {
            throw new Exception(ErrorMessages.InactiveUser);
        }
        bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!passwordValid)
        {
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
            throw new Exception(ErrorMessages.InvalidRefreshToken);
        }
        var newAccessToken = GenerateToken(storedToken.User);
        storedToken.IsRevoked=true;
        var newRefreshToken=GenerateRefreshToken();
        await _tokenRepository.AddRefreshTokenAsync(new RefreshToken
        {
            Token=refreshToken,
            ExpiryDate=DateTime.UtcNow.AddDays(7),
            UserId=storedToken.UserId
        });
        return new TokenResponseDto
        {
            AccessToken=newAccessToken,
            RefreshToken=newRefreshToken
        };
    }
    
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto dto)
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
            var user = await _userRepository.GetUserByEmailAsync(model.Email);

            if (user == null)
                throw new Exception(ErrorMessages.UserNotFound);

            // Hash Password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

            // Update DB
            await _userRepository.UpdatePasswordAsync(user);

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
