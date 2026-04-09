using MediCore.Api.DTOs.AppointmentDtos;
using MediCore.Api.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> GetFreeSlots([FromQuery] int doctorId, [FromQuery] DateOnly date)
        {
            try
            {
                var result = await _service.GetFreeSlots(doctorId, date);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
