using MediCore.Api.Services.PrescriptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Pharmacist,Admin")]
    public class PharmacyController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PharmacyController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet("queue")]
        public async Task<IActionResult> GetQueuedPrescriptions(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentException("pageNumber must be greater than or equal to 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException("pageSize must be greater than or equal to 1.");
            }
            var result =
                await _prescriptionService
                    .GetQueuedPrescriptionsAsync(pageNumber, pageSize);

            return Ok(result);
        }
    }
}
