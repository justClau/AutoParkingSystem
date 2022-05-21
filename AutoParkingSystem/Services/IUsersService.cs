using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
{
    public interface IUsersService
    {
        UsersResult ChangeUsername(string Username, string NewUsername);
        UsersResult GetInfo(int UserID);
        UsersResult Register(User User);
    }
}