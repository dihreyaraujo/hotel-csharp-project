using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms.Where(r => r.HotelId == HotelId).Include(r => r.Hotel).Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                Hotel = new HotelDto
                {
                    HotelId = HotelId,
                    Name = r.Hotel!.Name,
                    CityId = r.Hotel.CityId,
                    CityName = _context.Cities.Where(c => c.CityId == r.Hotel.CityId).Select(c => c.Name).FirstOrDefault(),
                    Address = r.Hotel.Address,
                    State = _context.Cities.Where(c => c.CityId == r.Hotel.CityId).Select(c => c.State).FirstOrDefault(),
                },
            });
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            var newRoom = _context.Rooms.Add(room);
            _context.SaveChanges();

            return new RoomDto
            {
                RoomId = newRoom.Entity.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = room.HotelId,
                    Name = _context.Hotels.Where(h => h.HotelId == room.HotelId).Select(h => h.Name).FirstOrDefault(),
                    CityId = _context.Hotels.Where(h => h.HotelId == room.HotelId).Select(h => h.CityId).FirstOrDefault(),
                    CityName = _context.Cities.Where(c => c.CityId == _context.Hotels.Where(h => h.HotelId == room.HotelId).Select(h => h.CityId).FirstOrDefault()).Select(c => c.Name).FirstOrDefault(),
                    Address = _context.Hotels.Where(h => h.HotelId == room.HotelId).Select(h => h.Address).FirstOrDefault(),
                    State = _context.Cities.Where(c => c.CityId == _context.Hotels.Where(h => h.HotelId == room.HotelId).Select(h => h.CityId).FirstOrDefault()).Select(c => c.State).FirstOrDefault(),
                }
            };
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId) {
            var room = _context.Rooms.Find(RoomId);
            if (room == null)
            {
                return;
            }
            _context.Rooms.Remove(room);
            _context.SaveChanges();
        }
    }
}