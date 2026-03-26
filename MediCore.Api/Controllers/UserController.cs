using MediCore.Api.DTOs.UserDtos;
using MediCore.Api.Services;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService serviceRepository;
        public UserController(IUserService repository)
        {
            serviceRepository = repository;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserResponseDto> users = await serviceRepository.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
