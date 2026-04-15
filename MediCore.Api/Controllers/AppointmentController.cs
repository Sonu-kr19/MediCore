using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Services.AppointmentServices;
using MediCore.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    // [Authorize(Roles ="Admin, Patient")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;
        public AppointmentController(IAppointmentService service)
        {
            _service=service;
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
                var (result, isNew) = await _service.BookAppointment(dto);
                // When new Idempotency Key is provided then response code will be 201 ok created.
                if (isNew)
                    return StatusCode(StatusCodes.Status201Created, result);  
                // If IdempotencyKey is same then it will return already existing appointment 
                return Ok(result);
            }
            catch(ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (MediCoreException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
