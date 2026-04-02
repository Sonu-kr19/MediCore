
using System;
using System.ComponentModel.DataAnnotations;
using MediCore.Domain.Enum;

namespace MediCore.Api.DTOs.UserDtos;


public class UserRegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string ConfirmPassword { get; set; }
    
    [Required]
    public RoleOption RoleName { get; set; }

    public string? Phone { get; set; }
}