using APSDataAccessLibrary.DAL.Repositories;

namespace APSDataAccessLibrary.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IBillsRepository Bills { get; }
        IParkingLotsRepository ParkingLots { get; }
        IUsersRepository Users { get; }
        IVehicleRepository Vehicles { get; }

        int Commit();
        ConfigDTO ConfigParkingLot();
    }
}