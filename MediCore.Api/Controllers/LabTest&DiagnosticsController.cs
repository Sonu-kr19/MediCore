using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.Repositories.LabTestRepository;
using MediCore.Api.Services.LabTestServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LabTestController : ControllerBase
    {
        private readonly ILabTestService _labTestService;
        public LabTestController(ILabTestService labTestService)
        {
           
            _labTestService = labTestService;
        }
    
        [HttpPost("lab/tests")]       
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLabTest(LabTestRequestDto labTest)
        {
            try
            {   
                var labTestID = await _labTestService.AddLabTestAsync(labTest);
                return Created($"/api/v1/lab/tests/{labTestID}", new { LabTestID = labTestID, Message = "Lab test created successfully." });
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
