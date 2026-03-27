using System;
using MediCore.Api.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Services.AuthServices;

public interface IAuthService
{
    Task <IActionResult> ForgotPasswordAsync(ForgotPasswordDto model);
}
