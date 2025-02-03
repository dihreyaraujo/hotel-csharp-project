using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return _context.Hotels.Select(c => new HotelDto
            {
                HotelId = c.HotelId,
                Name = c.Name,
                CityId = c.CityId,
                CityName = _context.Cities.Where(x => x.CityId == c.CityId).Select(x => x.Name).FirstOrDefault(),
                Address = c.Address,
                State = _context.Cities.Where(x => x.CityId == c.CityId).Select(x => x.State).FirstOrDefault(),
            });
        }
        
        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            var newHotel = _context.Hotels.Add(hotel);

            _context.SaveChanges();

            return new HotelDto
            {
                HotelId = newHotel.Entity.HotelId,
                Name = hotel.Name,
                CityId = hotel.CityId,
                CityName = _context.Cities.Where(x => x.CityId == hotel.CityId).Select(x => x.Name).FirstOrDefault(),
                Address = hotel.Address,
                State = _context.Cities.Where(x => x.CityId == hotel.CityId).Select(x => x.State).FirstOrDefault()
            };
        }
    }
}