using System;
using MediCore.Api.DTOs.PrescriptionDtos;
namespace MediCore.Api.Services.PrescriptionServices;

public interface IPrescriptionService
{
    Task<PrescriptionResponseDto> CreatePrescriptionAsync(PrescriptionRequestDto Request);
}
