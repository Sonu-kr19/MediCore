using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services;
using Microsoft.AspNetCore.Http;
using MediCore.Api.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Utilities;
 
namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
         public UserController(IUserService userService, IAuthService authService)
         {
            _userService = userService;
            _authService = authService;
         }
       
        [HttpGet("GetAll")]       
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {   
            List<UserResponseDto> users = await _userService.GetAllUsersAsync();
            return Ok(users);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("forgotpassword")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
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