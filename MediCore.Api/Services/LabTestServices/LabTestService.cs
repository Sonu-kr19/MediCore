using System;
using AutoMapper;
using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.Repositories.LabTestRepository;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Api.Services.LabTestServices;

public class LabTestService : ILabTestService
{
    private readonly ILabTestRepository _labTestRepository;
    private readonly IMapper _mapper;
    public LabTestService(ILabTestRepository labTestRepository, IMapper mapper)
    {
        _labTestRepository = labTestRepository;
        _mapper = mapper;
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
            // LabTest labTestEntity = new LabTest
            // {
            //     PatientID = dto.PatientID,
            //     DoctorID = id,
            //     Type = dto.Type,
            //     Date = dto.Date,
            //     TechnicianID = dto.TechnicianID,
            //     Status = dto.Status
            // };
            var labTestEntity = _mapper.Map<LabTestRequestDto, LabTest>(dto);
            labTestEntity.DoctorID = id;
           
            var result = await _labTestRepository.AddLabTestAsync(labTestEntity);
            return result.LabTestID;
         }
       catch (System.Exception ex)
       {
        throw new Exception(ex.Message);
       }
    }
    public async Task<List<LabTestResponseDto>> GetPendingLabTestsAsync()
    {
        List<LabTestResponseDto> labTestDtos = new List<LabTestResponseDto>();
        var labTests = await _labTestRepository.GetPendingLabTestsAsync();
        foreach (var labTest in labTests)
        {
            // labTestDtos.Add(new LabTestResponseDto
            // {
            //     LabTestID = labTest.LabTestID,
            //     PatientID = labTest.PatientID,
            //     DoctorID = labTest.DoctorID,
            //     Type = labTest.Type,
            //     Date = labTest.Date,
            //     TechnicianID = labTest.TechnicianID,
            //     Status = labTest.Status
            // });
            var labTestDto = _mapper.Map<LabTest,LabTestResponseDto>(labTest);
            labTestDtos.Add(labTestDto);
        }
        return labTestDtos;
    }
}

