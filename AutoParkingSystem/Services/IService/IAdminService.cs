using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IAdminService
    {
        AdminResults CreateUser(User User);
        AdminResults ShowUsers();
        AdminResults StartConfiguration();
        AdminResults ToggleAdmin(int UserID, bool AdminStatus = true);
    }
}