using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Vehicle GetVehicleByPlate(string Plate);
        Vehicle GetVehicleByVIN(string VIN);
    }
}