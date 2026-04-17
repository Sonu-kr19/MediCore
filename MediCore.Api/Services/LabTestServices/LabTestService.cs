using System;
using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.Repositories.LabTestRepository;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Services.LabTestServices;

public class LabTestService : ILabTestService
{
    private readonly ILabTestRepository _labTestRepository;
    public LabTestService(ILabTestRepository labTestRepository)
    {
        _labTestRepository = labTestRepository;
    }
    public async Task<int> AddLabTestAsync(LabTestRequestDto dto, int id)
    {
       try{
            if (dto.PatientID == 0)
            {
                throw new KeyNotFoundException(ErrorMessages.PatientIdNotFound);
            }
            var patient = await _labTestRepository.PatientExistsAsync(dto.PatientID);
            if (!patient)
            {
                throw new KeyNotFoundException(ErrorMessages.PatientNotFound);
            }
            var technician = await _labTestRepository.TechnicianExistsAsync(dto.TechnicianID);
            if (!technician)
            {
                throw new KeyNotFoundException(ErrorMessages.TechnicianNotFound);
            }
             LabTest labTestEntity = new LabTest
            {
                PatientID = dto.PatientID,
                DoctorID = id,
                Type = dto.Type,
                Date = dto.Date,
                TechnicianID = dto.TechnicianID,
                Status = dto.Status
            };
           
            var result = await _labTestRepository.AddLabTestAsync(labTestEntity);
            return result.LabTestID;
         }
       catch (System.Exception ex)
       {
        throw new Exception(ex.Message);
       }

    }
}
