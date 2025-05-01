using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers(){
            var token = HttpContext.User.Identity as ClaimsIdentity ?? throw new Exception("Invalid token");
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? throw new Exception("Invalid token");
            var users = _repository.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            var userExists = _repository.GetUserByEmail(user.Email);

            if (userExists != null)
            {
                return Conflict(new { message = "User email already exists" });
            }

            var newUser = _repository.Add(user);
            return Created("user", newUser);
        }
    }
}