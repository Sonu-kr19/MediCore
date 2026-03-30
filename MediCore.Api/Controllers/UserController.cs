using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services;
using Microsoft.AspNetCore.Http;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Services.UserServices;
using MediCore.Api.Services;
using MediCore.Api.Utilities;
 
namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
    
        // Injects IUserService via constructor injection so the controller
        // can delegate all user-related business logic to the service layer,
        // keeping the controller thin and testable.
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
 
        public UserController(IAuthService authService,IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        
        // Handles patient self-registration requests (POST api/v1/users/register/patient).
        // Separated from staff registration because patients and staff may have
        // different validation rules, roles, and onboarding workflows in the service layer.
        // Returns 200 on success, 400 for invalid input, 409 if the user already exists.
        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient(UserRegisterDto dto)
        {
            try
            {
                await _userService.RegisterPatientAsync(dto);
                return Ok(new { message = "Patient registered successfully." });
            }
            catch (ArgumentException ex)
            {
                // Catches validation failures (e.g. missing required fields, invalid email format)
                // thrown by the service and returns a 400 Bad Request with the error detail.
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Catches duplicate-user scenarios (e.g. email already registered)
                // thrown by the service and returns a 409 Conflict to signal the client
                // that the resource already exists rather than a generic error.
                return Conflict(new { error = ex.Message });
            }
        }

        // Handles staff registration requests (POST api/v1/users/register/staff).
        // Uses a dedicated endpoint so staff-specific logic (role assignment, permissions,
        // department linking) can evolve independently from patient registration
        // without breaking the patient flow.
        // Returns 200 on success, 400 for invalid input, 409 if the user already exists.
        [HttpPost("register/staff")]
        public async Task<IActionResult> RegisterStaff(UserRegisterDto dto)
        {
            try
            {
                await _userService.RegisterStaffAsync(dto);
                return Ok(new { message = "Staff registered successfully." });
            }
            catch (ArgumentException ex)
            {
                // Catches validation failures specific to staff input
                // and returns a descriptive 400 Bad Request to guide the caller.
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Catches duplicate staff registration attempts
                // and returns a 409 Conflict so the client knows to handle
                // the existing record rather than retrying the same request.
                return Conflict(new { error = ex.Message });
            }
        }
 
        [HttpPost("forgotpassword")]
        [ProduceResponseType(typeof(string), StatusCodes.Status200OK)];
        [ProduceResponseType(typeof(string), StatusCodes.Status400BadRequest)];
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {    
            try{
                var message = await _authService.ForgotPasswordAsync(dto);
                return Ok(new{message});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto) // Endpoint to update user details
        {
            
        //  Validate request body & required fields
         if (!ModelState.IsValid)
         {
            return BadRequest(ModelState);
         }
        try
        {
            await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(new { Message = ErrorMessage.Success });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}