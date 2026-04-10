using MediCore.Api.DTOs.EmrDtos;   
namespace MediCore.Api.Services.EMR;

public interface IEmrService
{
    Task<List<EmrResponseDto>> GetEmrAsync(int patientId);
    Task<EmrResponseDto> CreateEmrAsync(EmrRequestDto emrRequest);
}
