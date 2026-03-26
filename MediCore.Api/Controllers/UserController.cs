using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
 
namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
 
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }
 
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (model == null)
                return BadRequest("Invalid request");
 
            var (success, message) = await _authService.ForgotPasswordAsync(model);
 
            if (!success)
                return BadRequest(message);
 
            return Ok(new { message });
        }
    }
}