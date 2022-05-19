using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IVehicleRepository
    {
        Vehicle GetVehicleByPlate(string Plate);
        Vehicle GetVehicleByVIN(string VIN);
    }
}