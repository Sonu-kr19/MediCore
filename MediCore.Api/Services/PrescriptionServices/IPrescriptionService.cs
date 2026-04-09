
using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.PrescriptionDtos;

namespace MediCore.Api.Services.PrescriptionServices
{
    public interface IPrescriptionService
    {
        Task<PaginationResponseDto<QueuedPrescriptionDto>>
            GetQueuedPrescriptionsAsync(int pageNumber, int pageSize);
    }
}
