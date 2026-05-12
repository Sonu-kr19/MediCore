using System;
using MediCore.Api.DTOs.PaymentDtos;
using MediCore.Api.Repositories.PaymentRepo;
using MediCore.Api.Utilities;
using MediCore.Domain.Entities;

namespace MediCore.Api.Services.PaymentServices;

public class PaymentService:IPaymentService
{
    private readonly IPaymentRepository _repository;
    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentResponseDto> RecordPaymentAsync(PaymentRequestDto request)
    {
        // Idempotency check
        var existingPayment = await _repository.GetByReferenceAsync(request.PaymentReference);

        if (existingPayment != null)
        {
            return new PaymentResponseDto
            {
                PaymentID = existingPayment.PaymentID,
                PaymentReference = existingPayment.PaymentReference,
                Amount = existingPayment.Amount,
                Status = existingPayment.Status,
                Message = "Duplicate request. Returning existing payment."
            };
        }

        //Get Bill
        var bill = await _repository.GetBillByIdAsync(request.BillID);

        if (bill == null)
        {
            throw new Exception(ErrorMessage.NoBillFound);
        }

        //Create Payment
        var payment = new Payment
        {
            PaymentReference = request.PaymentReference,
            BillID = request.BillID,
            Amount = request.Amount,
            Date = DateTime.UtcNow,
            Method = request.Method,
            Status = true
        };

        await _repository.AddPaymentAsync(payment);

        //Update Bill PaidAmount
        bill.PaidAmount += request.Amount;

        //Check full payment
        if (bill.PaidAmount >= bill.Amount)
        {
            bill.Status = true; // Paid
        }
        else
        {
            bill.Status = false; // Unpaid

        //Underpayment logging
            var remaining = bill.Amount - bill.PaidAmount;
            Console.WriteLine($"Underpayment: Remaining {remaining}");
        }

        await _repository.UpdateBillAsync(bill);
        await _repository.SaveChangesAsync();

        return new PaymentResponseDto
        {
            PaymentID = payment.PaymentID,
            PaymentReference = payment.PaymentReference,
            Amount = payment.Amount,
            Status = payment.Status,
            Message = "Payment processed successfully"
        };
    }

}
