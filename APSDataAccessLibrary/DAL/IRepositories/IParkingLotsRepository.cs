using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IParkingLotsRepository : IRepository<ParkingLot>
    {
        ParkingLot AddVehicle(Vehicle vehicle);
        ParkingLot GetByName(int floorNumber, string name);
        ParkingLot GetByVehicle(int vehicleID);
        IEnumerable<ParkingLot> GetFree();
        IEnumerable<ParkingLot> GetFreeOnFloor(int floorNumber);
        ParkingLot RemoveVehicle(int vehicleID);
        ParkingLot SetVehicle(int parkingLotID, Vehicle vehicle);
        ParkingSchema GetParkingConfiguration();
    }
}