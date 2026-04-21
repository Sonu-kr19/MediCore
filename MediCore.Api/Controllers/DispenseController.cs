using MediCore.Api.DTOs.DispenseDtos;
using MediCore.Api.Services.DispenseServices;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Pharmacist)}")]
    public class DispenseController : ControllerBase
    {
        private readonly IDispenseService _dispenseService;

        public DispenseController(IDispenseService dispenseService)
        {
            _dispenseService = dispenseService;
        }

        /// <summary>
        /// Dispenses medicines for a prescription.
        /// Updates stock and prescription status.
        /// </summary>
        /// <param name="request">Dispense prescription request</param>
        /// <returns>Dispense result with optional warnings</returns>
       
        [HttpPost]
        public async Task<IActionResult> DispensePrescription(
        [FromBody] DispensePrescriptionRequestDto request)
        {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response =
                await _dispenseService.DispensePrescriptionAsync(request);

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        }

    }
}