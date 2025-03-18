using Hospital.Core.Service;
using Hospital.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userRepository;

        public UsersController(IUserService userRepository)
        {
            _userRepository = userRepository;
        }
        
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            bool success = await _userRepository.RegisterUser(request);
            if (!success)
                return BadRequest("User already exists");

            return Ok(new { message = "User registered successfully" });
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            string token = await _userRepository.LoginUser(request);
            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { token });
        }

        
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        
        [Authorize(Roles = "Admin,SuperAdmin")]        
        [HttpPost("{userId}/assign-role")]
        public async Task<IActionResult> AssignRole(int userId, [FromBody] string roleName)
        {
            await _userRepository.AssignRoleToUser(userId, roleName);
            return Ok(new { message = $"Role '{roleName}' assigned to user {userId}" });
        }

        
        //[Authorize(Roles = "SuperAdmin")]
        //[HttpPost("{userId}/assign-permission")]
        //public async Task<IActionResult> AssignPermission(int userId, [FromBody] string permission)
        //{
        //    await _userRepository.AssignPermissionToUser(userId, permission);
        //    return Ok(new { message = $"Permission '{permission}' assigned to user {userId}" });
        //}

        
        //[Authorize(Roles = "Admin,SuperAdmin")]
        //[HttpGet("{userId}/has-permission/{permission}")]
        //public async Task<IActionResult> CheckPermission(int userId, string permission)
        //{
        //    bool hasPermission = await _userRepository.HasPermission(userId, permission);
        //    return Ok(new { userId, permission, hasPermission });
        //}
    }
}
