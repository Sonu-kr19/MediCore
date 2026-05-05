using MediCore.Api.Services.ComplianceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCore.Domain.Enum;
using MediCore.Api.DTOs.ComplianceDtos;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Finance_Officer)}")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceService _complianceService;

        public ComplianceController(IComplianceService complianceService)
        {
            _complianceService = complianceService;
        }
        /// Creates a new compliance record.
        [HttpPost]
        public async Task<IActionResult> CreateComplianceRecord(CreateComplianceRecordRequestDto request)
        {
            // Missing required fields → 400
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _complianceService.CreateComplianceRecordAsync(request);
                // Return ComplianceId
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
