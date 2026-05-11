using MediCore.Api.DTOs.EmrDtos;   
using MediCore.Api.DTOs.LabReportDtos;
using Microsoft.AspNetCore.Mvc;
namespace MediCore.Api.Services.EMR;

public interface IEmrService
{
    Task<List<EmrResponseDto>>  GetEmrAsync(int patientId);
    Task<EmrResponseDto> CreateEmrAsync(EmrRequestDto emrRequest);
    Task<LabReportResponseDto> AttachLabReportAsync(LabReportRequestDto request);
}
