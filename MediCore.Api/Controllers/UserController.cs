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

        //Register User

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            try
            {
                await _userService.UserRegisterAsync(dto);

                // 200 OK — registration succeeded, return a success message.
                return Ok(new { message = "User registered successfully." });
            }
            catch (ArgumentException ex)
            {
                // Thrown by the service when input format is invalid (email, password, phone).
                // Mapped to 400 Bad Request — the client sent bad data and should fix it.
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Thrown by the service when the email is already registered.
                // Mapped to 409 Conflict — the resource already exists.
                return Conflict(new { error = ex.Message });
            }
        }
    }
}