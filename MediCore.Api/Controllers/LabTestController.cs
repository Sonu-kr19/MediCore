using MediCore.Api.DTOs.LabTestDto;
using MediCore.Api.Repositories.LabTestRepository;
using MediCore.Api.Services.LabTestServices;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestController : ControllerBase
    {
         private readonly ILabTestRepository _labTestRepository;
        private readonly ILabTestService _labTestService;
        public LabTestController(ILabTestRepository labTestRepository, ILabTestService labTestService)
        {
            _labTestRepository = labTestRepository;
            _labTestService = labTestService;
        }
    
        [HttpPost("lab/tests")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLabTest(LabTestRequestDto labTest)
        {
            try
            {   
                var labTestID = await _labTestService.AddLabTestAsync(labTest);
                return Ok(new { Message = "Lab test added successfully", LabTestID = labTestID });
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("lab/tests")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LabTest>))]
        public async Task<IActionResult> GetLabTests()
        {
            var labTests = await _labTestRepository.GetAllLabTestsAsync();
            return Ok(labTests);
        }
    }
}
