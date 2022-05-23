using AutoParkingSystem.BusinessLayer.DTO;
using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;
using AutoParkingSystem.BusinessLayer.Domain;

namespace AutoParkingSystem.BusinessLayer.Core
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork unit;

        public UsersService(IUnitOfWork unit)
        {
            this.unit = unit;
        }

        public UsersResult GetInfo(int UserID)
        {
            var user = unit.Users.Get(UserID);
            string[] Message = new string[4]
            {
                $"Welcome Back, {user.FullName}","","",""
            };
            if (user.Vehicle is not null)
            {
                var parkingLot = unit.ParkingLots.GetByVehicle(user.Vehicle.Id);
                var time = (DateTime.Now - user.Vehicle.StartingTime).TotalMinutes;
                Message[1] = $"Your Vehicle with Registration Number {user.Vehicle.PlateNumber}";
                Message[2] = $"Is parked at {parkingLot.Name} floor {parkingLot.FloorNumber}.";
                Message[3] = $"You parked your vehicle {time} minutes ago";
                return new UsersResult
                {
                    Success = true,
                    Message = $"{Message[0]}\n{Message[1]}\n{Message[2]}\n{Message[3]}"
                };
            }

            return new UsersResult
            {
                Success = true,
                Message = $"{Message[0]}\nYou don't have a parked car right now."
            };
        }
        public UsersResult ChangeUsername(string Username, string NewUsername)
        {
            if (Username.Equals(NewUsername))
                return new UsersResult
                {
                    Success = false,
                    Message = "The new username cannot be the same as the old one"
                };
            var user = unit.Users.GetByUsername(Username);
            user = unit.Users.UpdateUsername(user.Id, NewUsername);
            unit.Commit();
            return new UsersResult
            {
                Success = true,
                Message = "Username changed Successfuly!",
                User = user
            };
        }
        public UsersResult Register(User User)
        {
            User.IsAdmin = false;
            unit.Users.Add(User);
            unit.Commit();
            return new UsersResult
            {
                Success = true,
                Message = "You can now log in!",
                User = User
            };

        }

    }
}
