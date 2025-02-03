using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

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
            var roomById = _context.Rooms.First(room => room.RoomId == booking.RoomId);
            if (roomById.Capacity < booking.GuestQuant) {
                throw new Exception();
            }
            else
            {
                var userById = _context.Users.First(user => user.Email == email);
                var insertDb = _context.Bookings.Add(
                  new Booking
                  {
                      CheckIn = booking.CheckIn,
                      CheckOut = booking.CheckOut,
                      GuestQuant = booking.GuestQuant,
                      UserId = userById.UserId,
                      RoomId = booking.RoomId,
                  }
                );
                _context.SaveChanges();
                return GetBooking(insertDb.Entity.BookingId, email);
            }
        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings
                .Include(x => x.Room!.Hotel!.City)
                .First(x => x.BookingId.Equals(bookingId));

            var user = _context.Users.First(x => x.Email!.Equals(email));

            if (!user.UserId.Equals(booking.UserId))
            {
                throw new Exception();
            }

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto
                {
                    RoomId = booking.RoomId,
                    Name = booking.Room!.Name,
                    Capacity = booking.Room.Capacity,
                    Image = booking.Room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = booking.Room.HotelId,
                        Name = booking.Room.Hotel!.Name,
                        Address = booking.Room.Hotel.Address,
                        CityId = booking.Room.Hotel.CityId,
                        CityName = booking.Room.Hotel.City!.Name,
                        State = booking.Room.Hotel.City.State
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