using MediCore.Api.DTOs.PatientDtos;
using MediCore.Api.Services.PatientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    // Handles all HTTP requests related to patient operations (register, fetch, etc.)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        // Service layer dependency for patient business logic
        private readonly IPatientService _patientService;

        // Injects the patient service via constructor injection
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // Registers a new patient — open to all users, no login required
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(PatientRequestDto dto)
        {
            try
            {
                var patientId = await _patientService.RegisterPatientAsync(dto);

                // Returns 201 with a success message and the newly created patient ID
                return StatusCode(StatusCodes.Status201Created, new { message = "Patient registered successfully.", patientId });
            }
            catch (ArgumentException ex)
            {
                // Triggered when the request data is invalid (e.g. bad UserID, future DOB)
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Triggered when a conflict exists (e.g. InsuranceID already assigned to another patient)
                return Conflict(new { error = ex.Message });
            }
        }

        // Retrieves all patients — restricted to Admin roles only
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Fetches the full list of patients from the service layer
                var patients = await _patientService.GetAllPatientsAsync();
                return Ok(patients);
            }
            catch (ArgumentException ex)
            {
                // triggered when an invalid argument is passed (example .. bad filter/query param)
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // triggered when no patients exist or the operation cannot proceed in current state
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}