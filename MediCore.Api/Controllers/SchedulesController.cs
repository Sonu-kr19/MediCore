using MediCore.Api.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _service;
        public SchedulesController(IScheduleService service)
        {
            _service=service;
        }
        
        [HttpGet]
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
