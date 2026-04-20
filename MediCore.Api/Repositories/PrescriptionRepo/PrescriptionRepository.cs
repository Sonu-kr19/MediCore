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

        // Get queued (pending) prescriptions
        public async Task<List<Prescription>> GetQueuedPrescriptionsAsync(
            int pageNumber, int pageSize)
        {

            int skip = (pageNumber - 1) * pageSize;

            List<Prescription> pagedPrescriptions =
                await _context.Prescriptions
                    .Where(p => p.Status == false)
                    .OrderBy(p => p.Date)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

            return pagedPrescriptions;

        }

        // Get total count of queued prescriptions
        public async Task<int> GetQueuedPrescriptionsCountAsync()
        {
            int count = await _context.Prescriptions.CountAsync(p => p.Status == false);
            return count;
        }

        public async Task<Prescription> CreatePrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return await _context.Prescriptions
            .Include(p => p.PrescriptionItems)
            .FirstOrDefaultAsync(p => p.PrescriptionID == prescription.PrescriptionID);
        }
    }
}
