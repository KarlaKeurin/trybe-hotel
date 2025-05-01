using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<CityDto> GetCities()
        {
            return _context.Cities.Select(city => new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            }).ToList();            
        }

        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            return new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
            var cityToUpdate = _context.Cities.FirstOrDefault(c => c.CityId == city.CityId);
            if (cityToUpdate == null)
            {
                return null;
            }
            _context.SaveChanges();
            return new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };
        }

    }
}