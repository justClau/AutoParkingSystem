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
        private readonly IUsersService users;
        private readonly IValidationService validation;

        public UsersController(IUsersService users, IValidationService validation)
        {
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

        //GET: /api/users/new
        //Get INFO for registration
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

        //POST: /api/users/new
        //Register as a new user.
        [HttpPost("new")]
        public async Task<IActionResult> Register([FromBody]User User)
        {
            if (Username is not null && validation.UserExists(Username).Success == true)
                return BadRequest(new { Success = false, Message = "You are already logged in" });
            
            var user = validation.UserValidation(User);
            if (user.Success == false)
                return BadRequest(user);
           
            User.IsAdmin = false;
            return Ok(users.Register(User));
        }
    }
}
