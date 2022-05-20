﻿using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService admin;
        private readonly IValidationService validation;

        public AdminController(IAdminService admin, IValidationService validation)
        {
            this.admin = admin;
            this.validation = validation;
        }

        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        //GET: /api/admin
        //Check admin rights
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ok = validation.isAdmin(Username);
            if (ok.Success == false)
                return BadRequest(ok);
            return Ok(new { success = "true", message = $"Welcome back, {ok.Message}" });
        }

        //GET: /api/admin/users
        //See a list with all users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var ok = validation.isAdmin(Username);
            if (ok.Success == false)
                return BadRequest(ok);
            return Ok(admin.ShowUsers());
        }

        //POST: /api/admin/users
        //Create a new user
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var okAdmin = validation.isAdmin(Username);
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
        public async Task<IActionResult> UpdateUser([FromBody] string user, [FromHeader(Name = "action")] string Action)
        {
            var okAdmin = validation.isAdmin(Username);
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
        public async Task<IActionResult> Init()
        {
            var start = admin.StartConfiguration();
            if (start.Success)
                return Ok(start);
            return BadRequest(start);
        }
    }
}
