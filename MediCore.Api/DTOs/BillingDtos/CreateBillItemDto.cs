using System;

namespace MediCore.Api.DTOs.BillingDtos;

public class CreateBillItemDto
{
    public  string ItemName { get; set; }
    public decimal Rate { get; set; }
}
