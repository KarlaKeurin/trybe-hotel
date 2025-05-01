using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new Exception("User not found");
            var room = _context.Rooms.FirstOrDefault(r => r.RoomId == booking.RoomId) ?? throw new Exception("Room not found");
            var hotel = _context.Hotels.FirstOrDefault(h => h.HotelId == room.HotelId) ?? throw new Exception("Hotel not found");
            var city = _context.Cities.FirstOrDefault(c => c.CityId == hotel.CityId);

            if (booking.GuestQuant > room.Capacity)
            {
                return null;
            }

            var newBooking = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = user.UserId
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            return new BookingResponse
            {
                BookingId = newBooking.BookingId,
                CheckIn = newBooking.CheckIn,
                CheckOut = newBooking.CheckOut,
                GuestQuant = newBooking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                        State = city.State
                    }
                }
            };          
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings.Find(bookingId) ?? throw new Exception("Booking not found");
            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new Exception("User not found");
            if (booking.UserId != user.UserId)
            {
                return null;
            }

            var room = _context.Rooms.Find(booking.RoomId);
            var hotel = _context.Hotels.Find(room.HotelId);
            var city = _context.Cities.Find(hotel.CityId);

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = hotel.HotelId,
                        Name = hotel.Name,
                        Address = hotel.Address,
                        CityId = hotel.CityId,
                        CityName = city.Name,
                        State = city.State
                    }
                }
            };
        }

        public Room GetRoomById(int RoomId)
        {
            throw new NotImplementedException();
        }

    }

}