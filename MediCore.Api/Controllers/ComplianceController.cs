using MediCore.Api.Services.ComplianceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediCore.Domain.Enum;
using MediCore.Api.DTOs.ComplianceDtos;
using MediCore.Api.DTOs.PatientDtos;

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

        // Admin gets all pending compliance records.
        [Authorize(Roles = nameof(RoleOption.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("pending")]
        public async Task<IActionResult> GetAllPending()
        {
            var result = await _complianceService.GetAllPendingAsync();
            return Ok(result);
        }

        // Admin verifies a compliance record — Result must be Approved or Rejected.
        [Authorize(Roles = nameof(RoleOption.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id}/verify")]
        public async Task<IActionResult> Verify(int id, ComplianceVerifyDto dto)
        {
            var result = await _complianceService.VerifyAsync(id, dto);
            return Ok(result);
        }
    }
}
