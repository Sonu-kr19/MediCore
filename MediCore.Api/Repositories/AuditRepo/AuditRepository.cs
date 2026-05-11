using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.AuditRepo
{
    public class AuditRepository : IAuditRepository
    {
        private readonly MediCoreDbContext _context;

        public AuditRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Audit>> GetAuditsAsync(
            int? adminId,
            string? scope,
            string? findings,
            DateTime? fromDate,
            DateTime? toDate,
            bool? status)
        {
            var query = _context.Audits.AsQueryable();

            if (adminId.HasValue)
            {
                query = query.Where(a => a.AdminID == adminId.Value);
            }

            if (!string.IsNullOrWhiteSpace(scope))
            {
                query = query.Where(a => a.Scope == scope);
            }

            if (!string.IsNullOrWhiteSpace(findings))
            {
                query = query.Where(a => a.Findings == findings);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(a => a.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(a => a.Date <= toDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            return await query
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }
    }
}