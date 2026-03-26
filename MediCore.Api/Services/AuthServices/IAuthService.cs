using System;
using MediCore.Api.DTOs.UserDtos;

namespace MediCore.Api.Services.AuthServices;

public interface IAuthService
{
    Task <(bool Success, string Message)> ForgotPasswordAsync(ForgotPasswordDto model);
}
