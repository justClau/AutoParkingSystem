﻿using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDataAccess data;
        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        public AdminController(IDataAccess data)
        {
            this.data = data;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (Username is null) 
                return BadRequest(new { error = "true", message = "No username Specified!" });
            var usr = data.GetUserByUsername(Username);
            if (usr is null) return NotFound(new { error = "true", message = "Username is invalid" });
            if (!usr.IsAdmin) return BadRequest(new { error = "true", message = "You don't have the necessary permissions to access this route" });
            return Ok(new { success = "true", message = $"Welcome back, {usr.FullName}" });
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            if (Username is null)
                return BadRequest(new { error = "true", message = "No username Specified!" });
            var usr = data.GetUserByUsername(Username);
            if (usr is null) return NotFound(new { error = "true", message = "Username is invalid" });
            if (!usr.IsAdmin) return BadRequest(new { error = "true", message = "You don't have the necessary permissions to access this route" });
            var users = data.GetAllUsers();
            return Ok(new
            {
                error = false,
                message = "The users are loaded!",
                users = users
            });
        }
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!AdminVerify()) return BadRequest(new { error = "true", message = "Operation not allowed! Check username" });
            if (user == null) return BadRequest(new { error = "true", message = "No user specified" });
            if (data.GetUserByUsername(user.Username) is not null) return BadRequest(new { error = "true", message = "User already exists!" });
            var usr = data.AddUser(user);
            data.Commit();
            return Ok(new { success = "true", message = "User created successfully", user = usr });
        }
        [HttpPatch("users")]
        public async Task<IActionResult> UpdateUser([FromBody] string user, [FromHeader(Name = "action")] string Action)
        {
            if (!AdminVerify()) return BadRequest(new { error = "true", message = "Operation not allowed! Check username" });
            if (user == null) return BadRequest(new { error = "true", message = "No modifications specified" });
            var usr = data.GetUserByUsername(user);
            if (usr == null) return NotFound(new { error = "true", message = "User not found!" });
            if (Action.Equals("Admin"))
            {
                usr = data.SetAdmin(usr.Id, !usr.IsAdmin);
                data.Commit();
                return Ok(new { Success = "true", message = $"User is now: {usr.IsAdmin}" });
            }
            return BadRequest(new { error = "true", message = "No action atribute specified!" });
        }

        

        [HttpOptions]
        public async Task<IActionResult> Init()
        {
            var start = data.STARTCONFIG();
            if (start.Success)
                return Ok(start);
            return BadRequest(start);
        }
        private bool AdminVerify()
        {
            if (Username is null) return false;
            var usr = data.GetUserByUsername(Username);
            if (usr is null) return false;
            if (!usr.IsAdmin) return false;
            return true;
        }
    }
}
