using System;
using MediCore.Api.DTOs.PaymentDtos;

namespace MediCore.Api.Services.PaymentServices;

public interface IPaymentService
{
Task<PaymentResponseDto> RecordPaymentAsync(PaymentRequestDto request);
}
