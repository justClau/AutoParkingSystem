using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.DAL.Repositories;
using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public class AdminService
    {
        private readonly IUnitOfWork unit;

        public AdminService(IUnitOfWork unit)
        {
            this.unit = unit;
        }
        public AdminResults ShowUsers()
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
        public AdminResults ToggleAdmin(string Username)
        {
            var user = unit.Users.GetByUsername(Username);
            unit.Users.SetAdminStatus(user.Id, !user.IsAdmin);
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
    public class AdminResults
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<User>? AllUsers { get; set; }
    }
}
