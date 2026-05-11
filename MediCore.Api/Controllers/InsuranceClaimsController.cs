using System.Security.Claims;
using MediCore.Api.DTOs.InsuranceClaimDtos;
using MediCore.Api.Services;
using MediCore.Api.Utilities;
using MediCore.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Authorize(Roles = $"{nameof(RoleOption.Admin)},{nameof(RoleOption.Finance_Officer)}")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InsuranceClaimsController : ControllerBase
    {
        private readonly IInsuranceClaimService _service;
        public InsuranceClaimsController(IInsuranceClaimService service)
        {
            _service = service;
        } 
    [HttpPost]
    public async Task<IActionResult> CreateClaim([FromBody] CreateInsuranceClaimRequestDto request)
    {
        try
        {
           var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { message = ErrorMessage.NotAuthenticated });
            }
            int userId = int.Parse(userIdClaim.Value);
            InsuranceClaimResponseDto result = await _service.CreateClaimAsync(request, userId);
            return StatusCode(201, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    }
}
