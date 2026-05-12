using MediCore.Api.DTOs.Common;
using MediCore.Api.DTOs.LabReportDtos;
using MediCore.Api.Repositories.LabReportRepo;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.LabReportServices
{
    public class LabReportService : ILabReportService
    {
        private readonly ILabReportRepository _repository;

        public LabReportService(ILabReportRepository repository)
        {
            _repository = repository;
        }
        public async Task<LabReport> AddLabReportAsync(UploadLabReportDto labReportDto, int labTestId)
        {
            // Save file to disk
            var uploadsFolder = Path.Combine("wwwroot", "lab-reports");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{labReportDto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await labReportDto.File.CopyToAsync(stream);
            }

            // Manual mapping — no AutoMapper needed
            LabReport labReport = new LabReport
            {
                LabTestID = labTestId,
                FileURI = filePath,
                Status = false,
                Date = DateTime.UtcNow
            };

            await _repository.AddLabReportAsync(labReport);

            return labReport;
        }
        public async Task<LabReportResponseDto> AttachLabReportAsync(LabReportRequestDto request)
        {
            var newReport = new LabReport
            {
                LabTestID = request.LabTestID,
                FileURI = request.FileURI,
                Date = DateTime.UtcNow,
                Status = true
            };

            // await _repository.CreateLabReportAsync(newReport);

            return new LabReportResponseDto
            {
                LabReportID = newReport.LabReportID,
                LabTestID = newReport.LabTestID,
                FileURI = newReport.FileURI,
                Notes = request.Notes,
                Date = newReport.Date,
                Status = newReport.Status
            };
        }

        public async Task<PaginationResponseDto<QueuedLabReportDto>> GetQueuedLabReportsAsync(int pageNumber, int pageSize, int? labTestId)
        {
            var reports = await _repository.GetQueuedLabReportsAsync(pageNumber, pageSize, labTestId);
            int totalCount = await _repository.GetQueuedLabReportsCountAsync(labTestId);

            var dtoList = reports.Select(r => new QueuedLabReportDto
            {
                LabReportID = r.LabReportID,
                LabTestID = r.LabTestID,
                LabTestName = r.LabTestIDNavigator != null ? r.LabTestIDNavigator.Type : string.Empty,
                FileURI = r.FileURI,
                Date = r.Date,
                Status = r.Status
            }).ToList();

            return new PaginationResponseDto<QueuedLabReportDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = dtoList
            };
        }
    }
}
