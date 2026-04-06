using System;
using MediCore.Domain.Enum;

namespace MediCore.Api.DTOs.UserDtos;

public class UpdateUserDto
{
    public required string UserName { get; set; }
    public  required RoleOption RoleName { get; set; }
    public string Email{get;set;}
    public string? Phone { get; set; }
    public bool? Status { get; set; }
}
