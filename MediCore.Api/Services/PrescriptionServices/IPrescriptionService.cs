using System;
using MediCore.Api.DTOs.PrescriptionDtos;
using MediCore.Api.DTOs.Common;
namespace MediCore.Api.Services.PrescriptionServices
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto> CreatePrescriptionAsync(PrescriptionRequestDto Request);
        Task<PaginationResponseDto<QueuedPrescriptionDto>>GetQueuedPrescriptionsAsync(int pageNumber, int pageSize);
    }
}
