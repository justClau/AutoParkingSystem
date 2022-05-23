
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService admin;
        private readonly IValidationService validation;
        private readonly IConfiguration configuration;

        public AdminController(IAdminService admin, IValidationService validation, IConfiguration configuration)
        {
            this.admin = admin;
            this.validation = validation;
            this.configuration = configuration.GetSection("ParkingSystem");
        }

        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        //GET: /api/admin
        //Check admin rights
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ok = validation.IsAdmin(Username);
            if (ok.Success == false)
                return BadRequest(ok);
            return Ok(new { success = "true", message = $"Welcome back, {ok.Message}" });
        }

        //GET: /api/admin/users
        //See a list with all users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var ok = validation.IsAdmin(Username);
            if (ok.Success == false)
                return BadRequest(ok);
            return Ok(admin.GetUsers());
        }

        //POST: /api/admin/users
        //Create a new user
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var okAdmin = validation.IsAdmin(Username);
            if (okAdmin.Success == false)
                return BadRequest(okAdmin);
            var okUser = validation.UserValidation(user);
            if (okUser.Success == false)
                return BadRequest(okUser);
            return Ok(admin.CreateUser(user));
        }
        
        //PATCH: /api/admin/users
        //Update a user admin rights
        [HttpPatch("users")]
        public async Task<IActionResult> UpdateUser([FromBody] string user)
        {
            var okAdmin = validation.IsAdmin(Username);
            if (okAdmin.Success == false)
                return BadRequest(okAdmin);
            var okUser = validation.UserExists(user);
            if (okUser.Success == false)
                return BadRequest(okUser);
            return Ok(admin.ToggleAdmin(okUser.UserID, okUser.Admin));
        }

        //OPTIONS: /api/admin
        //Run the Parking Lots Table Configuration Engine
        [HttpOptions]
        public async Task<IActionResult> Init([FromHeader(Name = "password")]string Password)
        {
            var adminPassword = configuration.GetSection("Password").Value;
            if (string.IsNullOrEmpty(Password) || Password.Equals(adminPassword) == false)
                return BadRequest(new
                {
                    Success = false,
                    Message = "The Password required for database configuration is wrong! Please try again!"
                });
            var start = admin.StartConfiguration();
            if (start.Success)
                return Ok(start);
            return BadRequest(start);
        }
    }
}
