using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels
                .Select(hotel => new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = hotel.City != null ? hotel.City.Name : null,
                State = hotel.City != null ? hotel.City.State : null
            }).ToList();
        }
        
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            return new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = _context.Cities
                    .Where(city => city.CityId == hotel.CityId)
                    .Select(city => city.Name)
                    .FirstOrDefault(),
                State = _context.Cities
                    .Where(city => city.CityId == hotel.CityId)
                    .Select(city => city.State)
                    .FirstOrDefault()                
            };
        }
    }
}