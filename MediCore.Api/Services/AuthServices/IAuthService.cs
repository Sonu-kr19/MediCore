using System;
using MediCore.Api.DTOs.TokenDtos;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Services.AuthServices;

public interface IAuthService
{
    Task <IActionResult> ForgotPasswordAsync(ForgotPasswordDto model);
    Task<TokenResponseDto> ValidateUserAsync(UserLoginDto dto);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
}
