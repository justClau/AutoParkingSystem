using APSDataAccessLibrary.Context;
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
            if(Username is null) return BadRequest(new {error = "true", message = "No username Specified!"});
            var usr = data.GetUserByUsername(Username);
            if(usr is null) return NotFound(new {error = "true", message = "Username is invalid"});
            if (!usr.IsAdmin) return BadRequest(new { error = "true", message = "You don't have the necessary permissions to access this route" });
            return Ok(new {success = "true", message = $"Welcome back, {usr.FullName}"});
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
        public async Task<IActionResult> UpdateUser([FromBody] User user, [FromHeader(Name = "action")] string Action)
        {
            if (AdminVerify()) return BadRequest(new { error = "true", message = "Operation not allowed! Check username" });
            if (user == null) return BadRequest(new { error = "true", message = "No modifications specified" });
            var usr = data.GetUserByUsername(user.Username);
            if (usr == null) return NotFound(new { error = "true", message = "User not found!" });
            if (Action.Equals("Admin"))
            {
                usr = data.SetAdmin(usr.Id, !usr.IsAdmin);
                data.Commit();
                return Ok(new { Success = "true", message = $"User is now: {usr.IsAdmin}" });
            }
            return BadRequest(new { error = "true", message = "No action atribute specified!" });
        }

        [HttpGet("{CarString}")]
        public async Task<IActionResult> FindCar(string CarString)
        {
            if (CarString.Length != 17 && CarString.Length != 8 && CarString.Length != 7 && CarString.Length != 6)
                return BadRequest(new { error = true, message = "Search string is invalid" });
            if (CarString.Length == 17)
            {
                var veh1 = data.GetVehicleByVIN(CarString);
                var parkingLot1 = data.GetParkingLotByVehicle(veh1.Id);
                return Ok(new
                {
                    VehicleVIN = veh1.VIN,
                    VehicleLicensePlate = veh1.PlateNumber,
                    VehicleParkingDate = veh1.parkTime,
                    VehicleParkingSpot = parkingLot1.Name,
                    VehicleParkingFloor = parkingLot1.Floor
                });
            }
            var veh = data.GetVehicleByPlate(CarString);
            var parkingLot = data.GetParkingLotByVehicle(veh.Id);
            return Ok(new
            {
                VehicleVIN = veh.VIN,
                VehicleLicensePlate = veh.PlateNumber,
                VehicleParkingDate = veh.parkTime,
                VehicleParkingSpot = parkingLot.Name,
                VehicleParkingFloor = parkingLot.Floor
            });
                
        }

        [HttpOptions]
        public async Task<IActionResult> Init()
        {
            var start = data.STARTCONFIG();
            if(start.Success)
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
