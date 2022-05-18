using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLots : ControllerBase
    {
        private float price;
        private readonly IDataAccess data;
        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        public ParkingLots(IDataAccess data)
        {
            this.data = data;
            this.price = 0.9f;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var freelots = data.GetFreeParkingLots();
            if (Username is null) 
                return Ok(new
                {
                    Warning = "You are not registered! You can see the parking lots but you won't be able to park!",
                    freelots
                });
            var usr = data.GetUserByUsername(Username);
            if (usr == null) 
                return BadRequest(new
                {
                    error = true,
                    Message = "Username not found! Please try again!"
                });
            if (usr.IsAdmin) return Ok(data.GetParkingLots());
            return Ok(freelots);   
        }
        [HttpPost]
        public async Task<IActionResult> ParkVehicle(Vehicle Vehicle)
        {
            if (Username is null) return BadRequest(new { error = true, Message = "No Username Specified" });
            var usr = data.GetUserByUsername(Username);
            if (usr is null)
            {
                return BadRequest(new { error = true, Message = "User not found!" });
            }
            Vehicle.parkTime = DateTime.Now;
            if (Vehicle.PlateNumber is null || Vehicle.VIN is null)
                return BadRequest(new { error = true, Message = "Car data not supplied(Missing PlateNumer and/or VIN" });
            var veh = data.AddVehicle(Vehicle);
            var parkingLot = data.AddParkingLotVehicle(Vehicle);
            var usr2 = data.SetUserVehicle(usr.Id, Vehicle);
            data.Commit();
            return Ok(veh);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteVehicle()
        {
            if (Username is null) return BadRequest(new { error = true, Message = "No Username Specified" });
            var usr = data.GetUserByUsername(Username);
            if (usr is null)
            {
                return BadRequest(new { error = true, Message = "User not found!" });
            }
            if (usr.Vehicle is null) return NotFound(new { error = true, Message = "You don't have a parked vehicle!", usr.Vehicle.Id });
            var id = usr.Vehicle.Id;
            var Vehicle = data.GetVehicleById(id);
            var parkingLot = data.GetParkingLotByVehicle(id);
            var bill = new Bill
            {
                User = usr,
                ParkingLot = $"{parkingLot.Name} | Floor: {parkingLot.Floor}",
                VehiclePlate = Vehicle.PlateNumber,
                VehicleVIN = Vehicle.VIN,
                IssuedAt = DateTime.Now,
                ParkTime = Vehicle.parkTime,
                BillValue = (DateTime.Now - Vehicle.parkTime).TotalMinutes * this.price,
                IsPaid = true
            };
            data.SaveBill(bill);
            data.RemoveParkingLotVehicle(id);
            usr = data.RemoveUserVehicle(usr.Id);
            data.RemoveVehicle(id);
            data.Commit();
            return Ok(usr);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(data.GetAllUsers());
        }
    }
}
