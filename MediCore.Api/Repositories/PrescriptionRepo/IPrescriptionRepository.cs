using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.PrescriptionRepo
{
    public interface IPrescriptionRepository
    {
        Task<List<Prescription>> GetQueuedPrescriptionsAsync(int pageNumber, int pageSize);
        Task<int> GetQueuedPrescriptionsCountAsync();
    }
}