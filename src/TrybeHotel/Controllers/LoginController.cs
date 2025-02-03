using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        private readonly TokenGenerator _token;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
            _token = new TokenGenerator();
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login){
            try
            {
                var user = _repository.Login(login);
                var tokenResponse = _token.Generate(user);
                return Ok(new { token = tokenResponse });
            }
            catch
            {
                return Unauthorized(new { message = "Incorrect e-mail or password" });
            }
        }
    }
}