using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByUsername(string name);
        IEnumerable<User> GetRegulars();
        User RemoveVehicle(int UserID);
        User SetAdminStatus(int UserID, bool AdminStatus);
        User SetVehicle(int UserID, Vehicle NewVehicle);
        User UpdateUsername(int UserID, string NewUsername);
    }
}