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
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
            
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return new UserDto
            {
                UserId = newUser.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = newUser.UserType,
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.ToList();
            return users.Select(u => new UserDto
            {
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email,
            UserType = u.UserType
            });
        }

    }
}