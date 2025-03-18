using Hospital.Model.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] Login request)
        //{
        //    if (request.Username == "admin" && request.Password == "password123") 
        //    {
        //        var token = _jwtTokenService.GenerateToken(request.Username, "Admin");
        //        return Ok(new { Token = token });
        //    }
        //    return Unauthorized();
        //}
    }
}
