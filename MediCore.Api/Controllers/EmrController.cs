using System;
using MediCore.Api.Services.EMR;
using MediCore.Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.DTOs.EmrDtos;
using Microsoft.AspNetCore.Authorization;
using MediCore.Domain.Enum;
using MediCore.Api.DTOs.LabReportDtos;

namespace MediCore.Api.Controllers;

[Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Doctor)}")]
[ApiController]
[Route("api/v1/[controller]")]
public class EmrController : ControllerBase
{
    private readonly IEmrService _emrService;

    public EmrController(IEmrService emrService)
    {
        _emrService = emrService;
    }


    [HttpPost("CreateEmr")]
    public async Task<IActionResult> CreateEmr(
        [FromBody] EmrRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emrId = await _emrService.CreateEmrAsync(request);

        return Ok(new { EmrId = emrId });
    }

    [HttpGet("GetEmr")]
    public async Task<IActionResult> GetEmr([FromQuery] int? patientId)
    {
        try
        {
            if(patientId == null)
            {
                return BadRequest("PatientId is required");
            }
            var emrs = await _emrService.GetEmrAsync(patientId.Value);
            return Ok(emrs);
        }
        catch (Exception)
        {
            return StatusCode(500, ErrorMessages.EMRNotFound);
        }
    }

    [HttpPost("attach-lab-report")]
    public async Task<IActionResult> AttachLabReport([FromBody] LabReportRequestDto request)
    {
        try
        {
            var result = await _emrService.AttachLabReportAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
