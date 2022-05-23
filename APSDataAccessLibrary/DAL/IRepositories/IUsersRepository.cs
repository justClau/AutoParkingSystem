using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        User GetByUsername(string name);
        IEnumerable<User> GetRegulars();
        User RemoveVehicle(int userID);
        User SetAdminStatus(int userID, bool adminStatus);
        User SetVehicle(int userID, Vehicle newVehicle);
        User UpdateUsername(int userID, string newUsername);
    }
}