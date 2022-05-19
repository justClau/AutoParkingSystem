using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IUsersRepository
    {
        User GetByUsername(string name);
        IEnumerable<User> GetRegulars();
        User RemoveVehicle(int UserID);
        User SetAdminStatus(int UserID, bool AdminStatus);
        User SetVehicle(int UserID, Vehicle NewVehicle);
    }
}