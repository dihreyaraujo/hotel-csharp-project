using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            var usersByEmail = _context.Users.Where((user) => user.UserId == userId).First();
            return new UserDto {
                UserId = usersByEmail.UserId,
                Name = usersByEmail.Name,
                Email = usersByEmail.Email,
                UserType = usersByEmail.UserType
            };
        }

        public UserDto Login(LoginDto login)
        {
            var findUser = _context.Users.FirstOrDefault((user) => user.Email == login.Email && user.Password == login.Password);
            if (findUser == null) 
            {
                throw new InvalidOperationException();
            }
            else 
            {
                return new UserDto 
                {
                    UserId = findUser!.UserId,
                    Name = findUser!.Name,
                    Email = findUser!.Email,
                    UserType = findUser!.UserType
                };
            }
        }
        public UserDto Add(UserDtoInsert user)
        {   
            var userByEmail = _context.Users.FirstOrDefault((userDb) => userDb.Email == user.Email);

            if (userByEmail != null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                var newUser = new User 
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    UserType = "client"
                };

                var response = _context.Users.Add(newUser);
                _context.SaveChanges();

                return new UserDto
                {
                    UserId = response.Entity.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    UserType = "client"
                };
            }
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var usersByEmail = _context.Users.Where((user) => user.Email == userEmail).First();
            return new UserDto {
                UserId = usersByEmail.UserId,
                Name = usersByEmail.Name,
                Email = usersByEmail.Email,
                UserType = usersByEmail.UserType
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return _context.Users.Select((user) => new UserDto {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            });
        }

    }
}