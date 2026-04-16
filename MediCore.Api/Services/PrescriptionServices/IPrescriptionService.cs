using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.PrescriptionDtos;

namespace MediCore.Api.Services.PrescriptionServices
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto>
            CreatePrescriptionAsync(PrescriptionRequestDto request);

        Task<PaginationResponseDto<QueuedPrescriptionDto>>
            GetQueuedPrescriptionsAsync(
                int pageNumber,
                int pageSize,
                int? doctorId);
    }
}