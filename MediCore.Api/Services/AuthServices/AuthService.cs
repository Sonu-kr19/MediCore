using System;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.AuthServices;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    public AuthService(IUserRepository userRepository)
    {
        _userRepository=userRepository;
    }
    public async Task<User?> ValidateUserAsync(UserLoginDto dto)
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
        return user;
    }
}
