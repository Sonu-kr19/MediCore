using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        // Store compliance event after document upload — Result defaults to Pending.
        public async Task CreateAsync(int patientId, string type)
        {
            var record = new ComplianceRecord
            {
                PatientID = patientId,
                Type = type,
                Result = "Pending",
                Date = DateTime.UtcNow,
                Note = string.Empty
            };
            _context.ComplianceRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        // Returns all compliance records that are still pending verification.
        public async Task<List<ComplianceRecord>> GetAllPendingAsync()
        {
            return await _context.ComplianceRecords
                .Where(c => c.Result == "Pending")
                .Include(c => c.PatientIDNavigator)
                .ToListAsync();
        }

        // Fetch single compliance record by ID.
        public async Task<ComplianceRecord?> GetByIdAsync(int complianceRecordId)
        {
            return await _context.ComplianceRecords
                .FirstOrDefaultAsync(c => c.ComplianceRecordID == complianceRecordId);
        }

        // Update result and note — called by verify endpoint.
        public async Task UpdateAsync(ComplianceRecord record)
        {
            _context.ComplianceRecords.Update(record);
            await _context.SaveChangesAsync();
        }
    }
}