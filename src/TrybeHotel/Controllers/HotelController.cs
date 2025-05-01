using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("hotel")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _repository;
  
        public HotelController(IHotelRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult GetHotels(){
            var hotels = _repository.GetHotels();
            return Ok(hotels);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Admin")]
        public IActionResult PostHotel([FromBody] Hotel hotel){
            if (hotel == null)
            {
                return BadRequest();
            }

            var newHotel = _repository.AddHotel(hotel);
            return Created("hotel", newHotel);
        }

    }
}