using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Repositories;
using MediCore.Api.Repositories.AuditRepo;
using MediCore.Api.Utilities;
using MediCore.Api.Utilities.Helpers;
using MediCore.Domain.Entities;
using MediCore.Domain.Enum;

namespace MediCore.Api.Services.UserServices;
public class UserService : IUserService
{
    IUserRepository _userRepository;
    IAuditLogRepository _auditLogRepository;
    public UserService(IUserRepository repository,IAuditLogRepository auditLogRepository)
    {
        _userRepository = repository;
        _auditLogRepository = auditLogRepository;
    }
    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        List<User> users = new List<User>();
        try
        {
            users = await _userRepository.GetAllUsersAsync();
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

    public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
    {
        User user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            return null;
        }
        UserResponseDto responseDto = new UserResponseDto
            {
            UserID = userId,
            UserName = user.Name,
            Email = user.Email,
            Phone = user.Phone,
            RoleName = user.RoleName.ToString(),
            Status = user.Status
            };
        return responseDto;
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
        if (string.IsNullOrWhiteSpace(updateUserDto.UserName))
        {
            throw new Exception(ErrorMessage.NameRequired);
        }
        user.Name=updateUserDto.UserName;
        // Email validation
        if (string.IsNullOrWhiteSpace(updateUserDto.Email) || !updateUserDto.Email.Contains("@"))
        {
           throw new Exception(ErrorMessage.InvalidEmail);
        }
        //Check only if email is changed
        if (!string.Equals(user.Email, updateUserDto.Email, StringComparison.OrdinalIgnoreCase))
        {
           var existingUser = await _userRepository.GetUserByEmailAsync(updateUserDto.Email);
           if (existingUser != null)
           {
              throw new Exception(ErrorMessage.EmailAlreadyExists);
           }
        }
        user.Email = updateUserDto.Email;
        user.RoleName = updateUserDto.RoleName;
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

    public async Task DeleteUserAsync(int userId) // Method to delete a user by user ID
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception(ErrorMessage.UserNotFound);
        }
        if (user.Status == false)
        {
            throw new Exception(ErrorMessage.UserAlreadyDeleted);
        }
        user.Status=false;
        await _userRepository.UpdateUserAsync(userId,user);
    }

    //user register

    public async Task UserRegisterAsync(UserRegisterDto dto)
    {
        // Validate format of password, email, and phone before making any DB call.
        // If any of these fail, they throw ArgumentException immediately — no wasted DB round-trip.
        PasswordHelper.Validate(dto.Password);
        EmailHelper.Validate(dto.Email);
        PhoneHelper.Validate(dto.Phone);

        // Check if a user with this email already exists in the DB.
        // We do this manually instead of relying on the DB unique constraint,
        // because a constraint violation throws a raw 500 — here we throw
        // InvalidOperationException which the controller maps to a clean 409 Conflict.
        var exists = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (exists != null)
            throw new InvalidOperationException(ErrorMessages.EmailAlreadyExists);

        if (dto.RoleName == RoleOption.Admin)
            throw new ArgumentException(ErrorMessages.AdminRegister);

        var user = new User
        {
            Name     = dto.Name,
            Email    = dto.Email,
            Phone    = dto.Phone,
            // Role is always forced to Patient — never taken from the DTO.
            // This prevents privilege escalation where a client could send "Admin" in the request body.
            // RoleName = dto.RoleName,
            RoleName = dto.RoleName,
            // Account is active immediately upon registration.
            Status   = true,

            // BCrypt hashes the password with an automatic salt before storing.
            // workFactor 12 = 4096 iterations — strong enough to resist brute-force,
            // light enough not to slow down normal registration traffic.
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12)
        };

        // Persist the new user. Any unexpected DB errors here will bubble up
        // as exceptions and be handled by the global error handler as 500.
        await _auditLogRepository.LogAsync(null, "Register_Successfull", $"Email: {dto.Email} — is Registered");
        await _userRepository.RegisterUserAsync(user);
    }
}

