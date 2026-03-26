using MediCore.Api.DTOs.TokenDtos;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService=authService;
        }        

        [HttpPost("login")]
        // [ProducesResponseType(401)]
        // [ProducesResponseType(200)]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var token = await _authService.ValidateUserAsync(dto);
            if (token==null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenResponseDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(result);
        }

        [HttpGet("/")]
        public IActionResult HealthCheck()
        {
            return Ok("Api Working");
        }
    }
}
