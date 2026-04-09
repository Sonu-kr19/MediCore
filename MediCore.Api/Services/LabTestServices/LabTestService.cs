using System;
using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.Repositories.LabTestRepository;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.LabTestServices;

public class LabTestService : ILabTestService
{
private readonly ILabTestRepository _labTestRepository;
    private readonly MediCoreDbContext _context;
    public LabTestService(ILabTestRepository labTestRepository, MediCoreDbContext context)
    {
        _labTestRepository = labTestRepository;
        _context = context;
    }
    public async Task<int> AddLabTestAsync(LabTestRequestDto labTest)
    {

       
        if (labTest.PatientID == null)
        {
            throw new Exception(ErrorMessages.PatientIdNotFound);
        }
        Patient patient = await _context.Patients.FindAsync(labTest.PatientID);
        if (patient == null)
        {
            throw new Exception(ErrorMessages.PatientNotFound);
        }
        if (labTest.DoctorID == null)
        {
            throw new Exception(ErrorMessages.InvalidDoctorId);
        }
        User doctor = await _context.Users.FindAsync(labTest.DoctorID);
        if (doctor == null || doctor.RoleName.ToString() != "Doctor")
        {
            throw new Exception(ErrorMessages.DoctorNotFound);
        }
        if (labTest.TechnicianID != null)
        {
            User technician = await _context.Users.FindAsync(labTest.TechnicianID);
            if (technician == null || technician.RoleName.ToString() != "Lab_Technician")
            {
                throw new Exception(ErrorMessages.TechnicianNotFound);
            }
        }
        LabTest labTestEntity = new LabTest
        {
            PatientID = labTest.PatientID,
            DoctorID = labTest.DoctorID,
            Type = labTest.Type,
            Date = labTest.Date,
            TechnicianID = labTest.TechnicianID,
            Status = labTest.Status
        };
        await _labTestRepository.AddLabTestAsync(labTestEntity);
        return labTestEntity.LabTestID;

    }
}
