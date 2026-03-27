using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.UserServices;

public class UserService : IUserService
{
    IUserRepository userRepository;
    public UserService(IUserRepository repository)
    {
        userRepository = repository;
    }
    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        List<User> users = new List<User>();
        try
        {
        users = await userRepository.GetAllUsersAsync();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
       List<UserResponseDto> userResponseDtos = new List<UserResponseDto>();
       foreach(User user in users)
        {
            UserResponseDto responseDto = new UserResponseDto
            {
            UserID = user.UserID,
            UserName = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName.ToString(),
            Status = user.Status
            };
            userResponseDtos.Add(responseDto);
        }
        return userResponseDtos;
    }
}
