using MediCore.Api.Services.PrescriptionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [ApiController]
    [Route("pharmacy")]
    [Authorize(Roles = "Pharmacist,Admin")]
    public class PharmacyController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PharmacyController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // GET: /pharmacy/queue?pageNumber=1&pageSize=10
        [HttpGet("queue")]
        public async Task<IActionResult> GetQueuedPrescriptions(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result =
                await _prescriptionService
                    .GetQueuedPrescriptionsAsync(pageNumber, pageSize);

            return Ok(result);
        }
    }
}
