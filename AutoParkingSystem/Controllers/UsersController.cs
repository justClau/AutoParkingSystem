using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        private readonly IDataAccess data;

        public UsersController(IDataAccess data)
        {
            this.data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (Username is null)
                return BadRequest(new { error = true, message = "Please specify user." });
            var usr = data.GetUserByUsername(Username);
            if (usr == null)
                return BadRequest(new { error = true, message = "User not found!" });
            return Ok(new
            {
                error = false,
                message = $"Welcome back, {usr.FullName}",
                VehicleParkedAt = usr.Vehicle == null ? "You haven't parked any vehicle yet!" : usr.Vehicle.ParkTime.ToString()
            });
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeUserName([FromBody]string username)
        {
            if (Username.Equals(username))
                return BadRequest(new { error = true, message = "You cannot change to the same username" });
            return Ok(new { error = true, message = "Not yet implemented;" });
        }
        [HttpGet("new")]
        public async Task<IActionResult> RegisterInfo()
        {
            if(Username is not null)
                return BadRequest(new {error = true, message = "You cannot register if you are logged in"});
            return Ok(new
            {
                Username = "string, must be unique",
                FullName = "Your full name"
            });
        }

        [HttpPost("new")]
        public async Task<IActionResult> Register([FromBody]User User)
        {
            if(User.Username is null || User.FullName is null)
                return BadRequest(new 
                { 
                    error = true,
                    message = "Username or Full name missing!", 
                    User = new {
                        Username = "string, must be unique",
                        FullName = "Your full name"
                    } 
                });
            User.IsAdmin = false;
            var user = data.AddUser(User);
            return Ok(user);
        }
    }
}
