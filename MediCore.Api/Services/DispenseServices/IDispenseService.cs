using MediCore.Api.DTOs.DispenseDtos;

namespace MediCore.Api.Services.DispenseServices
{
    public interface IDispenseService
    {
        Task<DispensePrescriptionResponseDto>
            DispensePrescriptionAsync(DispensePrescriptionRequestDto request);
    }
}