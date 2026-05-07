using MediCore.Api.DTOs.AuditDtos;
using MediCore.Api.Services.AuditServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/compliance/audits")]
    [Authorize(Roles = "Admin,Finance_Officer")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAudits(
            [FromQuery] GetAuditRequestDto request)
        {
            var result = await _auditService.GetAuditsAsync(request);

            return Ok(result);
        }
    }
}