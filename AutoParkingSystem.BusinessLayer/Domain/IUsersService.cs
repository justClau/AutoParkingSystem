using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Core;

namespace AutoParkingSystem.BusinessLayer.Domain
{
    public interface IUsersService
    {
        UsersResult ChangeUsername(string Username, string NewUsername);
        UsersResult GetInfo(int UserID);
        UsersResult Register(User User);
    }
}