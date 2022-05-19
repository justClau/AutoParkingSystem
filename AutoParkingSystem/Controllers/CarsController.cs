using APSDataAccessLibrary.DbAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        private readonly IDataAccess data;
        public CarsController(IDataAccess data)
        {
            this.data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (Username is null || data.GetUserByUsername(Username).IsAdmin == false)
                return BadRequest(new
                {
                    error = true,
                    message = "You don't have access!"
                });
            var vehicles = data.GetParkedVehicles();
            return Ok(new
            {
                error = false,
                vehicles = vehicles
            });
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
                    VehicleParkingDate = veh1.ParkTime,
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
                VehicleParkingDate = veh.ParkTime,
                VehicleParkingSpot = parkingLot.Name,
                VehicleParkingFloor = parkingLot.Floor
            });

        }
    }
}