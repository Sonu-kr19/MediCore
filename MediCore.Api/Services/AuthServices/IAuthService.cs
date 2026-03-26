using System;
using MediCore.Api.DTOs.TokenDtos;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.AuthServices;

public interface IAuthService
{
    Task<TokenResponseDto?> ValidateUserAsync(UserLoginDto dto);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
}
