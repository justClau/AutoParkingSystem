using APSDataAccessLibrary.DAL;
using APSDataAccessLibrary.Models;

namespace AutoParkingSystem.Services
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
            var msg = $"Welcome Back, {user.FullName}";
            if (user.Vehicle is not null)
            {
                var parkingLot = unit.ParkingLots.GetByVehicle(user.Vehicle.Id);
                var time = (DateTime.Now - user.Vehicle.ParkTime).TotalMinutes;
                var msg2 = $"Your Vehicle with Registration Number {user.Vehicle.PlateNumber}";
                var msg3 = $"Is parked at {parkingLot.Name} floor {parkingLot.Floor}.";
                var msg4 = $"You parked your vehicle {time} minutes ago";
                return new UsersResult
                {
                    Success = true,
                    Message = $"{msg}\n{msg2}\n{msg3}\n{msg4}"
                };
            }

            return new UsersResult
            {
                Success = true,
                Message = $"{msg}\nYou don't have a parked car right now."
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

    public class UsersResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User User { get; set; }
    }
}
