using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.ComplianceRepo
{
    public interface IComplianceRepository
    {
        Task AddComplianceRecordAsync(ComplianceRecord record);

        Task SaveChangesAsync();
    }
}