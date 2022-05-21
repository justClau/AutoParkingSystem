using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDataAccess data;
        private readonly IUsersService users;
        private readonly IValidationService validation;

        public UsersController(IDataAccess data, IUsersService users, IValidationService validation)
        {
            this.data = data;
            this.users = users;
            this.validation = validation;
        }

        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        //GET: /api/users
        //ACTION: Check if current user exists and has any parked\
        //Vehicle
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);
            return Ok(users.GetInfo(user.UserID));
        }

        //PATCH: /api/users/
        //Change current user's username
        [HttpPatch]
        public async Task<IActionResult> ChangeUserName([FromBody]string username)
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);
            
            var status = users.ChangeUsername(Username, username);
            if (status.Success == false)
                return BadRequest(status);
            
            return Ok(status);
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
