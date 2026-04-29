using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.DispenseRepo
{
    public class DispenseRepository : IDispenseRepository
    {
        private readonly MediCoreDbContext _context;

        public DispenseRepository(MediCoreDbContext context)
        {
            _context = context;
        }

        // Load prescription + prescription items
        public async Task<Prescription?> GetPrescriptionWithItemsAsync(int prescriptionId)
        {
            return await _context.Prescriptions
                .Include(p => p.PrescriptionItems)
                .FirstOrDefaultAsync(p => p.PrescriptionID == prescriptionId);
        }
        
        // Get medicine by name (string-based design)
        public async Task<Medicine?> GetMedicineByNameAsync(string medicineName)
        {
            return await _context.Medicines
                .FirstOrDefaultAsync(m => m.Name == medicineName);
        }

        // Update medicine stock
        public Task UpdateMedicineAsync(Medicine medicine)
        {
            _context.Medicines.Update(medicine);
            return Task.CompletedTask;
        }

        // Update prescription status
        public Task UpdatePrescriptionAsync(Prescription prescription)
        {
            _context.Prescriptions.Update(prescription);
            return Task.CompletedTask;
        }

        // Persist all changes
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}