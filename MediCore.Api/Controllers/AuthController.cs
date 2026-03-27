using MediCore.Api.DTOs.TokenDtos;
using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    /// <summary>
    /// AuthController for authentication
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService=authService;
        }        

        /// <summary>
        /// Api endpoint to login user for existing user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Returns AccessToken and Refresh Token as response for current loggedin user</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TokenResponseDto),StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            // Validating User details and Generating the token for Valid user.
            try
            {
                var token = await _authService.ValidateUserAsync(dto);
                return Ok(token);
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// Api Endpoint for Refresh token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Returns new Access token as a response</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TokenResponseDto),StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken(TokenResponseDto dto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(result);  
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
