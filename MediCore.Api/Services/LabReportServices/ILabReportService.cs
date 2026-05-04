using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.LabReportDtos;

namespace MediCore.Api.Services.LabReportServices
{
    public interface ILabReportService
    {
        Task<LabReportResponseDto> AttachLabReportAsync(LabReportRequestDto request);
        Task<PaginationResponseDto<QueuedLabReportDto>> GetQueuedLabReportsAsync(int pageNumber, int pageSize, int? labTestId);
    }
}
