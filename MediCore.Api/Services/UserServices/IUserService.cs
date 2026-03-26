using MediCore.Api.DTOs.UserDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services;
public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();
}
