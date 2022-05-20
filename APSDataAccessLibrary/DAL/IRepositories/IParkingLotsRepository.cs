using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IParkingLotsRepository : IRepository<ParkingLot>
    {
        ParkingLot AddVehicle(Vehicle Vehicle);
        ParkingLot GetByName(int FloorNumber, string Name);
        ParkingLot GetByVehicle(int VehicleID);
        IEnumerable<ParkingLot> GetFree();
        IEnumerable<ParkingLot> GetFreeOnFloor(int FloorNumber);
        ParkingLot RemoveVehicle(int VehicleID);
        ParkingLot SetVehicle(int ParkingLotID, Vehicle Vehicle);
        ParkingSchema GetParkingConfiguration();
    }
}