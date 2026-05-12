using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.AuditRepo
{
    public interface IAuditRepository
    {
        Task<List<Audit>> GetAuditsAsync(
            int? adminId,
            string? scope,
            string? findings,
            DateTime? fromDate,
            DateTime? toDate,
            bool? status
        );
    }
}