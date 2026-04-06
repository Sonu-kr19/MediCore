
using MediCore.Api.DTOs.UserDtos;

namespace MediCore.Api.Services.EMR;

public interface IEmrService
{
    Task<List<EmrDto>> GetEmrAsync(int patientId);
}
