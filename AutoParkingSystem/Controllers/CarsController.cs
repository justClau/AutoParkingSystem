using AutoParkingSystem.BusinessLayer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AutoParkingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IValidationService validation;
        private readonly IVehicleService vehicle;

        [FromHeader(Name = "username")]
        public string? Username { get; set; }

        public CarsController(IValidationService validation, IVehicleService vehicle)
        {
            this.validation = validation;
            this.vehicle = vehicle;
        }

        //GET: /api/cars
        //SHOW ALL PARKED VEHICLES
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = validation.IsAdmin(Username);
            if (user.Success == false)
                return BadRequest(user);

            return Ok(vehicle.ShowVehicles());
        }

        //GET /api/cars
        //SEARCH FOR A CAR
        [HttpGet("{CarString}")]
        public async Task<IActionResult> FindCar(string CarString)
        {
            var user = validation.IsAdmin(Username);
            if (user.Success == false)
                return BadRequest(user);

            var search = validation.SearchTerm(CarString);
            if (search.Success == false)
                return BadRequest(search);

            return Ok(vehicle.ShowVehicle(CarString, search.SearchType));
        }
    }
}