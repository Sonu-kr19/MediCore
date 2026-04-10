using System;
using MediCore.Api.Services.EMR;
using MediCore.Api.Utilities;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.DTOs.EmrDtos;
using Microsoft.AspNetCore.Authorization;

namespace MediCore.Api.Controllers;

[Authorize(Roles = "Doctor")]
[ApiController]
[Route("api/v1/[controller]")]
public class EmrController : ControllerBase
{
    private readonly IEmrService _emrService;

    public EmrController(IEmrService emrService)
    {
        _emrService = emrService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateEmr(
        [FromBody] EmrRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var emrId = await _emrService.CreateEmrAsync(request);

        return Ok(new { EmrId = emrId });
    }

    [HttpGet]
    public async Task<IActionResult> GetEmr([FromQuery] int patientId)
    {
        try
        {
            var emrs = await _emrService.GetEmrAsync(patientId);
            return Ok(emrs);
        }
        catch (Exception)
        {
            return StatusCode(500, ErrorMessages.EMRNotFound);
        }
    }
}
