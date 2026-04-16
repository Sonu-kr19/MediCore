using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Repositories.PrescriptionRepo
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly MediCoreDbContext _context;

        public PrescriptionRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        // ✅ Get queued (pending) prescriptions with optional doctor filter
        public async Task<List<Prescription>> GetQueuedPrescriptionsAsync(
            int pageNumber,
            int pageSize,
            int? doctorId)
        {
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<Prescription> query = _context.Prescriptions
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionItems)
                .Where(p => p.Status == false); // queued only

            // ✅ Apply doctor filter if provided
            if (doctorId.HasValue)
            {
                query = query.Where(p => p.DoctorID == doctorId.Value);
            }

            return await query
                .OrderBy(p => p.Date)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        // ✅ Get total count of queued prescriptions with optional doctor filter
        public async Task<int> GetQueuedPrescriptionsCountAsync(int? doctorId)
        {
            IQueryable<Prescription> query =
                _context.Prescriptions.Where(p => p.Status == false);

            if (doctorId.HasValue)
            {
                query = query.Where(p => p.DoctorID == doctorId.Value);
            }

            return await query.CountAsync();
        }

        // ✅ Create prescription with items (single transaction)
        public async Task CreatePrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }
    }
}