using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingLotsController : ControllerBase
    {
        private readonly IParkingService parking;
        private readonly IValidationService validation;
        private readonly IBillingService billing;

        public ParkingLotsController(IParkingService parking, IValidationService validation, IBillingService billing)
        {
            this.parking = parking;
            this.validation = validation;
            this.billing = billing;
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

            if (validation.IsAdmin(Username).Success == true)
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

            var vehicle = validation.VerifyCarDetails(Vehicle);
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

            vehicle.Bill = billing.CreateBill(user.UserID, vehicle.VehicleInfo, vehicle.Message).Bill;
            return Ok(vehicle);
        }

        //GET: /api/ParkingLots/{NumarEtaj}
        //Show all free parking lots on the specified floor
        [HttpGet("{Floor}")]
        public async Task<IActionResult> FloorInformation(int Floor)
        {
            var user = validation.UserExists(Username);
            if (user.Success == false)
                return BadRequest(user);
            var floor = parking.GetFloorInformation(Floor);
            if (floor.Success == false)
                return BadRequest(floor);
            return Ok(floor);
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
