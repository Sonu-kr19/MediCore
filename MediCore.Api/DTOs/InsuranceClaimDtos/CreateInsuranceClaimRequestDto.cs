using System;

namespace MediCore.Api.DTOs.InsuranceClaimDtos;

public class CreateInsuranceClaimRequestDto
{
    public int BillID { get; set; }
    public int InsuranceID { get; set; }
    public decimal Amount { get; set; }
}
