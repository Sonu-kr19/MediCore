using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services.AuthServices;
using MediCore.Api.Utilities.TokenUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService=authService;
            _tokenService=tokenService;   
        }

        [HttpPost("login")]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var isValid = await _authService.ValidateUserAsync(dto);
            if (!isValid)
            {
                return Unauthorized("Invalid credentials");
            }
            var token = _tokenService.GenerateToken(dto.Email);
            return Ok(new {token});
        }
    }
}
