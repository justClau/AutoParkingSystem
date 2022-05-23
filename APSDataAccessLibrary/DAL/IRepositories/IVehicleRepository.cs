using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Vehicle GetVehicleByPlate(string plate);
        Vehicle GetVehicleByVIN(string vin);
    }
}