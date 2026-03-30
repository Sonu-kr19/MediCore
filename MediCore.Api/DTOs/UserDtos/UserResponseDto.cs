using System;

namespace MediCore.Api.DTOs.UserDtos;

public class UserResponseDto
{
    public int UserID { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string RoleName { get; set; }
    public required string Phone { get; set; }
    public bool Status { get; set; }
}
