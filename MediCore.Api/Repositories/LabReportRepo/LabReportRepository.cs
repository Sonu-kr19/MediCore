using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.LabReportRepo
{
    public class LabReportRepository : ILabReportRepository
    {
        private readonly MediCoreDbContext _context;

        public LabReportRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        public async Task CreateLabReportAsync(LabReport report)
        {
            _context.LabReports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LabReport>> GetQueuedLabReportsAsync(int pageNumber, int pageSize, int? labTestId)
        {
            var query = _context.LabReports.AsQueryable();

            if (labTestId.HasValue)
                query = query.Where(r => r.LabTestID == labTestId.Value);

            return await query
                .OrderByDescending(r => r.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetQueuedLabReportsCountAsync(int? labTestId)
        {
            var query = _context.LabReports.AsQueryable();
            if (labTestId.HasValue)
                query = query.Where(r => r.LabTestID == labTestId.Value);

            return await query.CountAsync();
        }
    }
}
