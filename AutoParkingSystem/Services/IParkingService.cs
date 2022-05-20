using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IParkingService
    {
        int GetParkingLotID(int FloorNumber, string Name);
        ParkingResults Park(int UserID, Vehicle Vehicle, int ParkingLotID);
        ParkingResults ParkToNearestFreeSpace(int UserID, Vehicle Vehicle);
        ParkingResults ShowAllParkingLots();
        ParkingResults ShowFreeParkingLots();
        ParkingResults UnPark(int UserID);
    }
}