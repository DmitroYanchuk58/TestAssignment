using ApplicationTier.Interfaces;
using ApplicationTier.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationTier.DTO;

namespace PresentationTier.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _service;

        public UsersController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody]UserForRegister model)
        {
            try
            {
                User newUser = new User(Guid.Empty, model.Username, model.Email, model.Password, null);
                _service.Register(newUser);

                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]UserForLogin model)
        {
            try
            {
                if (_service.Authenticate(model.Username, model.Password))
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
