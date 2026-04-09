using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    // [Authorize]
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
            try
            {
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
    }
}
