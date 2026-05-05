using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.ComplianceRepo
{
    public class ComplianceRepository : IComplianceRepository
    {
        private readonly MediCoreDbContext _context;

        public ComplianceRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        /// Adds a new compliance record to the database.
        public async Task AddComplianceRecordAsync(ComplianceRecord record)
        {
            await _context.ComplianceRecords.AddAsync(record);
        }
        
        /// Saves all pending changes.
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}