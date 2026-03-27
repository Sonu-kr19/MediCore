
using MediCore.Api.DTOs.UserDtos;

namespace MediCore.Api.Services.UserServices
{
    public interface IUserService
    {
        Task RegisterPatientAsync(UserRegisterDto dto);
        Task RegisterStaffAsync(UserRegisterDto dto);
    }
}