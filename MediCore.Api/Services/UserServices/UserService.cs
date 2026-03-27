using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Services.UserServices;
public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;
     public UserService(IUserRepository userRepository) // Constructor injection of the user repository
    {
        _userRepository = userRepository;
    }
     public async Task<UserResponseDto?> GetUserByIdAsync(int userId) // Method to get user details by user ID
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception(ErrorMessage.UserNotFound);
        }
        return new UserResponseDto
        {
            UserID = user.UserID,
            UserName = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName.ToString(),
            Status = user.Status
        };
    }
    public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto) // Method to update user details
    {
        
        if (updateUserDto == null)
        {
            throw new Exception(ErrorMessage.UpdateUserRequest);
        }
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new Exception(ErrorMessage.UserNotFound);
        }
        if(updateUserDto.UserID!=id)
        {
            throw new Exception(ErrorMessage.InvalidAction);
        }
        if (string.IsNullOrWhiteSpace(updateUserDto.UserName))
        {
            throw new Exception(ErrorMessage.NameRequired);
        }
        user.Name=updateUserDto.UserName;
        if (!Enum.IsDefined(typeof(RoleOption), updateUserDto.RoleName))
        {
           throw new Exception(ErrorMessage.InvalidRoleName);
        }
        user.RoleName = updateUserDto.RoleName;
        if (string.IsNullOrWhiteSpace(updateUserDto.Email) || !updateUserDto.Email.Contains("@"))
        {
         throw new Exception(ErrorMessage.InvalidEmail);
        }
        if (updateUserDto.Phone != null)
        {
           user.Phone = updateUserDto.Phone;
        }
        if (updateUserDto.Status != null)
        {
          user.Status = (bool)updateUserDto.Status;
        }
        try
        {
            await _userRepository.UpdateUserAsync(id, user);
        }
        catch
        {
            throw new Exception(ErrorMessage.UpdateFailedUser);
        }
    }
}
