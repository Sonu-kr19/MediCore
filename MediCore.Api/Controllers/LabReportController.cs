using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Services.LabReportServices;
using MediCore.Api.DTOs.LabReportDtos;
using System.Threading.Tasks;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabReportController : ControllerBase
    {
        private readonly ILabReportService _service;

        public LabReportController(ILabReportService service)
        {
            _service = service;
        }

        [HttpPost("attach")]
        public async Task<IActionResult> AttachLabReport([FromBody] LabReportRequestDto request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var result = await _service.AttachLabReportAsync(request);
            return Ok(result);
        }

        [HttpGet("queued")]
        public async Task<IActionResult> GetQueuedLabReports(int pageNumber = 1, int pageSize = 10, int? labTestId = null)
        {
            var result = await _service.GetQueuedLabReportsAsync(pageNumber, pageSize, labTestId);
            return Ok(result);
        }
    }
}
