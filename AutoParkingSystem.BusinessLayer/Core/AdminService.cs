using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.DAL.Repositories;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;
using AutoParkingSystem.BusinessLayer.DTO;

namespace AutoParkingSystem.BusinessLayer.Core
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unit;

        public AdminService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public AdminResults GetUsers()
        {
            return new AdminResults
            {
                Success = true,
                AllUsers = unit.Users.GetAll()
            };
        }
        public AdminResults CreateUser(User User)
        {
            unit.Users.Add(User);
            unit.Commit();
            return new AdminResults
            {
                Success = true,
                Message = $"User {User.FullName} has been created with username '{User.Username}'"
            };
        }
        public AdminResults ToggleAdmin(int UserID, bool AdminStatus = true)
        {
            var user = unit.Users.SetAdminStatus(UserID, !AdminStatus);
            unit.Commit();
            return new AdminResults
            {
                Success = true,
                Message = $"User {user.Username} is now {(user.IsAdmin ? "admin" : "regular user")}"
            };
        }
        public AdminResults StartConfiguration()
        {
            var start = unit.ConfigParkingLot();
            return new AdminResults
            {
                Success = start.Success
            };
        }
    }
}
