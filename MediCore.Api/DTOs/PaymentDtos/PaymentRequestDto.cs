using System;
using MediCore.Domain.Enum;

namespace MediCore.Api.DTOs.PaymentDtos;

public class PaymentRequestDto
{

    public string PaymentReference { get; set; }
    public int BillID { get; set; }
    public decimal Amount { get; set; }
    public PaymentOption Method { get; set; }

}
