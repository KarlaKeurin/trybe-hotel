using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
            var token = HttpContext.User.Identity as ClaimsIdentity ?? throw new Exception("Invalid token");
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? throw new Exception("Invalid token");
            var response = _repository.Add(bookingInsert, email);

            if (bookingInsert == null)
            {
                return BadRequest(new { message = "Invalid booking data" });
            }

            return Created("booking", response);
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
            var token = HttpContext.User.Identity as ClaimsIdentity ?? throw new Exception("Invalid token");
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? throw new Exception("Invalid token");
            var bookings = _repository.GetBooking(Bookingid, email);

            if (bookings == null)
            {
                return Unauthorized();
            }

            return Ok(bookings);
        }
    }
}