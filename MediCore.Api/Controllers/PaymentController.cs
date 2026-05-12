using MediCore.Api.DTOs.PaymentDtos;
using MediCore.Api.Services.PaymentServices;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Finance_Officer)}")]
    public class PaymentController : ControllerBase
    {
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> RecordPayment([FromBody] PaymentRequestDto request)
    {
        try
        {
            var response = await _paymentService.RecordPaymentAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    }
}
