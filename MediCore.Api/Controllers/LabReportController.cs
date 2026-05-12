using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Services.LabReportServices;
using MediCore.Api.DTOs.LabReportDtos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MediCore.Domain.Enum;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LabReportController : ControllerBase
    {
        private readonly ILabReportService _service;

        public LabReportController(ILabReportService service)
        {
            _service = service;
        }
        [Authorize(Roles = nameof(RoleOption.Lab_Technician))]
        [HttpPost("lab/tests/{id}/report")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadLabReport(
    [FromForm] UploadLabReportDto labReportDto,
    int id)
        {
            if (labReportDto.File == null || labReportDto.File.Length == 0)
                return BadRequest(new { error = "No file provided." });

            var allowedTypes = new[] { "application/pdf", "image/jpeg", "image/png" };
            if (!allowedTypes.Contains(labReportDto.File.ContentType))
                return BadRequest(new { error = "Only PDF, JPG and PNG allowed." });

            if (labReportDto.File.Length > 10 * 1024 * 1024)
                return BadRequest(new { error = "File must be under 10MB." });

            try
            {
                var labReport = await _service.AddLabReportAsync(labReportDto, id);
                return CreatedAtAction(
                    nameof(UploadLabReport),
                    new { id = labReport.LabReportID },
                    new { labReport.LabReportID, labReport.FileURI, labReport.Date }
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
