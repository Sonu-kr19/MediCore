using System;
using MediCore.Api.DTOs.BillingDtos;

namespace MediCore.Api.Services;

public interface IBillService
{
   Task<int> CreateBillAsync(CreateBillDto dto);
}
