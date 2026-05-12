using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.ComplianceRepo
{
    public interface IComplianceRepository
    {
        Task AddComplianceRecordAsync(ComplianceRecord record);

        Task SaveChangesAsync();
        
        Task CreateAsync(int patientId, string type);
        Task<List<ComplianceRecord>> GetAllPendingAsync();
        Task<ComplianceRecord?> GetByIdAsync(int complianceRecordId);
        Task UpdateAsync(ComplianceRecord record);
    }
}