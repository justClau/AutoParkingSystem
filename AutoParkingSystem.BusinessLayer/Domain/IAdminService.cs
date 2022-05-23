using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IAdminService
    {
        AdminResults CreateUser(User User);
        AdminResults GetUsers();
        AdminResults StartConfiguration();
        AdminResults ToggleAdmin(int UserID, bool AdminStatus = true);
    }
}