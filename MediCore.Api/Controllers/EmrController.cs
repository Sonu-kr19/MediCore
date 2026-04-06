using System;
using MediCore.Api.Services.EMR;
using MediCore.Api.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmrController : ControllerBase
{
    private readonly IEmrService _emrService;

    public EmrController(IEmrService emrService)
    {
        _emrService = emrService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmr([FromQuery] int patientId)
    {
        try
        {
            var emrs = await _emrService.GetEmrAsync(patientId);
            return Ok(emrs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ErrorMessages.EMRNotFound);
        }
    }
}
