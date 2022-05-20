using APSDataAccessLibrary.DbAccess;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private int count = -1;
        private float price;
        private readonly IDataAccess data;
        private readonly IParkingService parking;
        private readonly IValidationService validation;

        public ParkingLotsController(IParkingService parking, IValidationService validation)
        {
            this.data = data;
            this.price = 0.09f;
            this.parking = parking;
            this.validation = validation;
        }

        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        //GET: /api/parkinglots
        //Show All Parking Lots
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);

            if (validation.isAdmin(Username).Success == true)
                return Ok(parking.ShowAllParkingLots());

            return Ok(parking.ShowFreeParkingLots());
        }

        //POST: /api/parkinglots
        //Park a vehicle to the nearest free parking place!
        [HttpPost]
        public async Task<IActionResult> ParkVehicle(Vehicle Vehicle)
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);

            var parkinglots = parking.ShowFreeParkingLots();
            if (parkinglots.Success == false)
                return BadRequest(parkinglots);

            var vehicle = validation.CarDetails(Vehicle);
            if(vehicle.Success == false)
                return BadRequest(vehicle);

            return Ok(parking.ParkToNearestFreeSpace(user.UserID, Vehicle));
        }

        //DELETE: /api/parkinglots
        //Unpark a vehicle and Generate Bill
        [HttpDelete]
        public async Task<IActionResult> DeleteVehicle()
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);
            
            var vehicle = parking.UnPark(user.UserID);
            if(vehicle.Success==false)
                return BadRequest(vehicle);

            //TODO BILL SERVICE!
            return Ok(vehicle);

            //if (Username is null) return BadRequest(new { error = true, Message = "No Username Specified" });
            //var usr = data.GetUserByUsername(Username);
            //if (usr is null)
            //{
            //    return BadRequest(new { error = true, Message = "User not found!" });
            //}
            //if (usr.Vehicle is null) return NotFound(new { error = true, Message = "You don't have a parked vehicle!", usr.Vehicle.Id });
            //var id = usr.Vehicle.Id;
            //var Vehicle = data.GetVehicleById(id);
            //var parkingLot = data.GetParkingLotByVehicle(id);
            //var bill = new Bill
            //{
            //    User = usr,
            //    ParkingLot = $"{parkingLot.Name}|{parkingLot.Floor}",
            //    VehiclePlate = Vehicle.PlateNumber,
            //    VehicleVIN = Vehicle.VIN,
            //    IssuedAt = DateTime.Now,
            //    ParkTime = Vehicle.ParkTime,
            //    BillValue = (DateTime.Now - Vehicle.ParkTime).TotalMinutes * this.price,
            //    IsPaid = true
            //};
            //data.SaveBill(bill);
            //data.RemoveParkingLotVehicle(id);
            //usr = data.RemoveUserVehicle(usr.Id);
            //data.RemoveVehicle(id);
            //data.Commit();
            //return Ok(usr);
        }
        
        //GET: /api/parkinglots/{NumarEtaj}/{ParkingLotName}
        //Show info about a specific parking lot
        [HttpGet("{Floor}/{ParkingLotName}")]
        public async Task<IActionResult> ParkingLotInformation(int Floor, string ParkingLotName)
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);

            var parkingLot = validation.ParkingLotName(Floor, ParkingLotName);
            if (parkingLot.Success == false)
                return BadRequest(parkingLot);
            return Ok(parkingLot);
        }

        //POST: /api/parkinglots/{NumarEtaj}/{ParkingLotName}
        //Park to a specific Parking Lot
        [HttpPost("{Floor}/{ParkingLotName}")]
        public async Task<IActionResult> ParkToSpecifiedParkingLot(int Floor, string ParkingLotName, [FromBody]Vehicle Vehicle)
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);

            var parkingLot = validation.ParkingLotName(Floor, ParkingLotName);
            if (parkingLot.Success == false)
                return BadRequest(parkingLot);

            return Ok(parking.Park(user.UserID, Vehicle, parking.GetParkingLotID(Floor, ParkingLotName)));
        }

    }
}
