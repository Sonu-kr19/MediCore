using MediCore.Api.DTOs.UserDtos;

namespace MediCore.Api.Services;
public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();

}
