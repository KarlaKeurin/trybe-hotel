using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId) {
            return _context.Rooms
                .Where(room => room.HotelId == HotelId)
                .Select(room => new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = room.Hotel.City.Name,
                        State = room.Hotel.City.State
                    },
                }).ToList();
        }

        public RoomDto AddRoom(Room room) {
           _context.Rooms.Add(room);
            _context.SaveChanges();

            var hotelDto = _context.Hotels
                .Where(hotel => hotel.HotelId == room.HotelId)
                .Select(hotel => new HotelDto
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
                })
                .FirstOrDefault();

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = hotelDto
            };
        }

        public void DeleteRoom(int RoomId) {
            var room = _context.Rooms.Find(RoomId);
            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return;
        }
    }
}