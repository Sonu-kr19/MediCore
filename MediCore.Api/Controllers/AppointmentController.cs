using System.Security.Claims;
using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Services;
using MediCore.Api.Services.AppointmentServices;
using MediCore.Api.Services.PatientServices;
using MediCore.Api.Utilities;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Patient)}")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;
        private readonly IPatientService _patientService;
        public AppointmentController(IAppointmentService service, IPatientService patientService)
        {
            _service=service;
            _patientService=patientService;
        }
        
        [HttpGet("Schedules")]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ScheduleResponseDto),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFreeSlots([FromQuery] int doctorId, [FromQuery] DateOnly date)
        {
            try{
                var result = await _service.GetFreeSlots(doctorId, date);
                return Ok(result);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(AppointmentResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateAppointment(AppointmentRequestDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized("Invalid user identifier.");
                }
                var patient = await _patientService.GetByIdAsync(userId);
                var (result, isNew) = await _service.BookAppointment(patient.PatientID, dto);
                // When new Idempotency Key is provided then response code will be 201 ok created.
                if (isNew)
                    return StatusCode(StatusCodes.Status201Created, result);  
                // If IdempotencyKey is same then it will return already existing appointment 
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (MediCoreException ex)
            {
                return BadRequest(ex.Message);
            }
            // catch (Exception ex)
            // {
            //     return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            // }
        }
        
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _service.CancelAppointmentAsync(id);
                return Ok(new CancelAppointmentResponseDto
                {
                    AppointmentId = id,
                    Message = "Appointment cancelled successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}/reschedule")]
        public async Task<IActionResult> Reschedule(int id, [FromBody] RescheduleRequestDto dto)
        {
            try
            {
                var result = await _service.RescheduleAppointmentAsync(id, dto);
                if (!result) return NotFound();
                return Accepted(new { status = true, message = "Update request accepted." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
