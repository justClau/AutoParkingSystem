using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.DAL;
using AutoParkingSystem.BusinessLayer.Domain;

namespace AutoParkingSystem.BusinessLayer.Core
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
}
