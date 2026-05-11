using System;
using MediCore.Api.DTOs.InsuranceClaimDtos;

namespace MediCore.Api.Services;

public interface IInsuranceClaimService
{
Task<InsuranceClaimResponseDto> CreateClaimAsync(CreateInsuranceClaimRequestDto dto, int userId);
}
