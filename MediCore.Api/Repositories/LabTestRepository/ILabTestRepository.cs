using System;
using MediCore.Domain.Entities;

namespace MediCore.Api.Repositories.LabTestRepository;

public interface ILabTestRepository
{
    Task<LabTest> AddLabTestAsync(LabTest labTest);
    Task<bool> DoctorExistsAsync(int doctorId);
    Task<bool> PatientExistsAsync(int patientId);
    Task<bool> TechnicianExistsAsync(int technicianId);
    Task<List<LabTest>> GetPendingLabTestsAsync();

}
