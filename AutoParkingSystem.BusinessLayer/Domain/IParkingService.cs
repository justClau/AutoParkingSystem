using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IParkingService
    {
        int GetParkingLotID(int FloorNumber, string Name);
        ParkingResults Park(int UserID, Vehicle Vehicle, int ParkingLotID);
        ParkingResults ParkToNearestFreeSpace(int UserID, Vehicle Vehicle);
        ParkingResults ShowAllParkingLots();
        ParkingResults ShowFreeParkingLots();
        ParkingResults UnPark(int UserID);
        ParkingResults GetFloorInformation(int FloorNumber);
    }
}