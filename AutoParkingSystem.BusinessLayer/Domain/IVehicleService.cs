using AutoParkingSystem.BusinessLayer.DTO;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IVehicleService
    {
        VehicleResult ShowVehicle(string SearchTerm, SearchType search);
        VehicleResult ShowVehicles();
    }
}