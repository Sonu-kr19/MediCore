using System;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.AuthServices;

public interface IAuthService
{
    Task<User?> ValidateUserAsync(UserLoginDto dto);
}
