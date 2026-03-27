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
    }
}