
using MediCore.Api.DTOs.UserDtos;
namespace MediCore.Api.Services;
public interface IUserService
{
    Task RegisterPatientAsync(UserRegisterDto dto);
    Task RegisterStaffAsync(UserRegisterDto dto);
}
