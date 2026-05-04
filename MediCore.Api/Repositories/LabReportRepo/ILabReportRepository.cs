using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.LabReportRepo
{
    public interface ILabReportRepository
    {
        Task CreateLabReportAsync(LabReport report);
        Task<IEnumerable<LabReport>> GetQueuedLabReportsAsync(int pageNumber, int pageSize, int? labTestId);
        Task<int> GetQueuedLabReportsCountAsync(int? labTestId);
    }
}
