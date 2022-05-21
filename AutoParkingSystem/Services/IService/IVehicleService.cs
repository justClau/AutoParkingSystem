namespace AutoParkingSystem.Services
{
    public interface IVehicleService
    {
        VehicleResult ShowVehicle(string SearchTerm, SearchType search);
        VehicleResult ShowVehicles();
    }
}