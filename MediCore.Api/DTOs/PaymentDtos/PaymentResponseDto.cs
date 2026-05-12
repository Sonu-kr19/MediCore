using System;

namespace MediCore.Api.DTOs.PaymentDtos;

public class PaymentResponseDto
{

    public int PaymentID { get; set; }
    public string PaymentReference { get; set; }
    public decimal Amount { get; set; }
    public bool Status { get; set; }
    public string Message { get; set; }

}
