using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService _userService;
         public UserController(IUserService userService)
        {
            _userService = userService;
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
    }
}
