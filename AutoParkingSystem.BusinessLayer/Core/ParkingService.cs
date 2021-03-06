using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;

namespace AutoParkingSystem.BusinessLayer.Core
{
    public class ParkingService : IParkingService
    {
        private readonly IUnitOfWork unit;

        public ParkingService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public ParkingResults ShowFreeParkingLots()
        {
            var parkinglots = unit.ParkingLots.GetFree();
            var count = parkinglots.Count();
            if (count < 1)
                return new ParkingResults
                {
                    Success = false,
                    Message = "No parking available"
                };
            return new ParkingResults
            {
                Success = true,
                Count = count,
                ParkingLots = parkinglots
            };
        }
        public ParkingResults ShowAllParkingLots()
        {
            return new ParkingResults
            {
                Success = true,
                ParkingLots = unit.ParkingLots.GetAll()
            };
        }
        public int GetParkingLotID(int FloorNumber, string Name)
        {
            var parkingLot = unit.ParkingLots.GetByName(FloorNumber, Name);
            return parkingLot.Id;
        }
        public ParkingResults ParkToNearestFreeSpace(int UserID, Vehicle Vehicle)
        {
            var user = unit.Users.Get(UserID);
            if (user.Vehicle is not null)
                return new ParkingResults
                {
                    Success = false,
                    Message = "Operation not allowed! You Already have a vehicle"
                };

            Vehicle.StartingTime = DateTime.Now;
            user = unit.Users.SetVehicle(UserID, Vehicle);
            unit.Commit();
            unit.ParkingLots.AddVehicle(user.Vehicle);
            unit.Commit();
            return new ParkingResults
            {
                Success = true,
                Message = $"Vehicle with id {user.Vehicle.Id} has been parked!"
            };
        }
        public ParkingResults Park(int UserID, Vehicle Vehicle, int ParkingLotID)
        {
            var user = unit.Users.Get(UserID);
            if (user.Vehicle is not null)
                return new ParkingResults
                {
                    Success = false,
                    Message = "Operation not allowed! You Already have a vehicle"
                };

            Vehicle.StartingTime = DateTime.Now;
            user = unit.Users.SetVehicle(UserID, Vehicle);
            unit.ParkingLots.SetVehicle(ParkingLotID, Vehicle);
            unit.Commit();
            return new ParkingResults
            {
                Success = true,
                Message = $"Vehicle with id {user.Vehicle.Id} has been parked!"
            };
        }
        public ParkingResults UnPark(int UserID)
        {
            var user = unit.Users.Get(UserID);
            if (user.Vehicle is null)
                return new ParkingResults
                {
                    Success = false,
                    Message = "Operations not allowed! You don't have any parked vehicles"
                };
            var Vehicle = user.Vehicle;
            DateTime StartTime = user.Vehicle.StartingTime;
            unit.Users.RemoveVehicle(Vehicle.Id);
            var parkingLot = unit.ParkingLots.RemoveVehicle(Vehicle.Id);
            unit.Vehicles.Remove(Vehicle);
            unit.Commit();
            return new ParkingResults
            {
                Success = true,
                Message = $"{parkingLot.FloorNumber}/{parkingLot.Name}",
                VehicleInfo = Vehicle
            };
        }
        public ParkingResults GetFloorInformation(int FloorNumber)
        {
            var variabila1 = unit.ParkingLots.GetParkingConfiguration();
            if (variabila1.Floors <= FloorNumber)
                return new ParkingResults
                {
                    Success = false,
                    Message = "Floor Numer is out of range! Please Try again"
                };
            var floor = unit.ParkingLots.GetFreeOnFloor(FloorNumber);
            return new ParkingResults
            {
                Success = true,
                ParkingLots = floor
            };
        }
    }
}
