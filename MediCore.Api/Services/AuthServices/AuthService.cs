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

namespace MediCore.Api.Services.AuthServices;

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
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
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
    public async Task<TokenResponseDto?> ValidateUserAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (user==null)
        {
            return null;
        }
        bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
        if (!passwordValid)
        {
            return null;
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
    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _tokenRepository.GetRefreshTokenAsync(refreshToken);
        if(storedToken==null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
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
}
