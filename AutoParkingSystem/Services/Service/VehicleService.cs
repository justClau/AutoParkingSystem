using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork unit;

        public VehicleService(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        public VehicleResult ShowVehicles()
        {
            return new VehicleResult
            {
                Success = true,
                Vehicles = unit.Vehicles.GetAll()
            };
        }
        public VehicleResult ShowVehicle(string SearchTerm, SearchType search)
        {
            if (search == SearchType.PlateNumber)
            {
                var vehicle = unit.Vehicles.GetVehicleByPlate(SearchTerm);
                var parkinglot = unit.ParkingLots.GetByVehicle(vehicle.Id);
                return new VehicleResult
                {
                    Success = true,
                    Vehicle = vehicle,
                };
            }
            else
            {
                var vehicle = unit.Vehicles.GetVehicleByVIN(SearchTerm);
                var parkinglot = unit.ParkingLots.GetByVehicle(vehicle.Id);
                return new VehicleResult
                {
                    Success = true,
                    Vehicle = vehicle,
                };
            }
        }
    }
    public enum SearchType
    {
        PlateNumber,
        VIN
    }

    public class VehicleResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Vehicle? Vehicle { get; set; }
        public IEnumerable<Vehicle>? Vehicles { get; set; }
    }
}
