using MediCore.Api.DTOs.BillingDtos;
using MediCore.Api.Services;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Finance_Officer)}")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        
    private readonly IBillService _service;
    public BillController(IBillService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillDto dto)
    {
        try
        {
            int billId = await _service.CreateBillAsync(dto);

            BillResponseDto response = new BillResponseDto();
            response.BillID = billId;
            int itemCount = dto.BillItems.Count;
            if (itemCount == 1)
            {
                response.Message = "Bill created successfully and 1 bill item added.";
            }
            else
            {
                response.Message = "Bill created successfully and " + itemCount + " bill items added.";
            }
                return Created("", response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    }
}
