using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.DispenseRepo
{
    public interface IDispenseRepository
    {
        Task<Prescription?> GetPrescriptionWithItemsAsync(int prescriptionId);

        Task<Medicine?> GetMedicineByNameAsync(string medicineName);

        Task UpdateMedicineAsync(Medicine medicine);

        Task UpdatePrescriptionAsync(Prescription prescription);

        Task SaveChangesAsync();
    }
}