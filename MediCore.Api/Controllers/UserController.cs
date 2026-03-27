using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services;
using MediCore.Api.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) // Constructor injection of the user service
        {
            _userService = userService;
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
}
