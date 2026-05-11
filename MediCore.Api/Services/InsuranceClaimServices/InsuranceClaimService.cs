using System;
using MediCore.Api.DTOs.InsuranceClaimDtos;
using MediCore.Api.Repositories.InsuranceClaimRepo;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services;

public class InsuranceClaimService:IInsuranceClaimService
{ 
        private readonly IInsuranceClaimRepository _repository;
        public InsuranceClaimService(IInsuranceClaimRepository repository)
        {
            _repository = repository;
        }
        public async Task<InsuranceClaimResponseDto> CreateClaimAsync(CreateInsuranceClaimRequestDto dto,int userId)
        {
            if (!await _repository.InsuranceExistsAsync(dto.InsuranceID))
            {
                throw new ArgumentException(ErrorMessage.InvalidInsuranceId);
            }

            if (!await _repository.BillExistsAsync(dto.BillID))
            {
                throw new ArgumentException(ErrorMessage.BillNotFound);
            }

            if (await _repository.ClaimExistsForBillAsync(dto.BillID))
            {
                throw new InvalidOperationException(ErrorMessage.DuplicateClaim);
            }

            InsuranceClaim claim = new InsuranceClaim
            {
                BillID = dto.BillID,
                InsuranceID = dto.InsuranceID,
                Amount = dto.Amount,
                UserID = userId,
                Status = "Submitted",
                Date = DateTime.UtcNow
            };
            InsuranceClaim savedClaim = await _repository.CreateAsync(claim);
            return new InsuranceClaimResponseDto
            {
                ClaimID = savedClaim.ClaimID,
                BillID = savedClaim.BillID,
                InsuranceID = savedClaim.InsuranceID,
                Amount = savedClaim.Amount,
                Status = savedClaim.Status,
                Date = savedClaim.Date
            };
        }
}
