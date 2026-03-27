using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Utilities;
 
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            try
            {
                var message = await _authService.ForgotPasswordAsync(model);
                return Ok(new{message});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}