using System;

namespace MediCore.Api.DTOs.UserDtos;

public class ForgotPasswordDto
{
    public string Email {get; set; }
    public string NewPassword {get; set; }
    public string ConfirmPassword {get; set; }
}
