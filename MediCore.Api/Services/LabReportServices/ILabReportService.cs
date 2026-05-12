using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.LabReportDtos;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.LabReportServices
{
    public interface ILabReportService
    {
        Task<LabReport> AddLabReportAsync(UploadLabReportDto labReportDto, int labTestId);
        Task<LabReportResponseDto> AttachLabReportAsync(LabReportRequestDto request);
        Task<PaginationResponseDto<QueuedLabReportDto>> GetQueuedLabReportsAsync(int pageNumber, int pageSize, int? labTestId);
    }
}
