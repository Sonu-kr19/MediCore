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
            List<Prescription> allPrescriptions =
                await _context.Prescriptions.ToListAsync();

            List<Prescription> queuedPrescriptions =
                new List<Prescription>();

            // Filter queued prescriptions (Status == false)
            foreach (Prescription prescription in allPrescriptions)
            {
                if (prescription.Status == false)
                {
                    queuedPrescriptions.Add(prescription);
                }
            }

            // Pagination logic (simple math, no LINQ)
            List<Prescription> pagedPrescriptions =
                new List<Prescription>();

            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = startIndex + pageSize;

            for (int i = startIndex; i < endIndex && i < queuedPrescriptions.Count; i++)
            {
                pagedPrescriptions.Add(queuedPrescriptions[i]);
            }

            return pagedPrescriptions;
        }

        // Get total count of queued prescriptions
        public async Task<int> GetQueuedPrescriptionsCountAsync()
        {
            List<Prescription> allPrescriptions =
                await _context.Prescriptions.ToListAsync();

            int count = 0;

            foreach (Prescription prescription in allPrescriptions)
            {
                if (prescription.Status == false)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
