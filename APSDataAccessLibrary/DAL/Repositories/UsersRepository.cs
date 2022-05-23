using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public ParkingContext ParkingContext { get { return context as ParkingContext; } }
        public UsersRepository(ParkingContext context) : base(context) { }

        public IEnumerable<User> GetRegulars()
        {
            var query = from user in ParkingContext.Users.Include(u => u.Vehicle)
                        where user.IsAdmin == false
                        orderby user.Username ascending
                        select user;
            return query;
        }
        public override User Get(int userID)
        {
            var user = ParkingContext.Users.Find(userID);
            ParkingContext.Entry(user).Reference(u => u.Vehicle).Load();
            return user;
        }
        public override IEnumerable<User> GetAll()
        {
            return ParkingContext.Users.Include(u => u.Vehicle).ToList();
        }
        public User GetByUsername(string name)
        {
            return ParkingContext.Users.Include(u => u.Vehicle).FirstOrDefault(u => u.Username == name);
        }
        public User SetAdminStatus(int userID, bool adminStatus)
        {
            var user = ParkingContext.Users.Find(userID);
            if (user == null)
                return null;
            user.IsAdmin = adminStatus;
            return user;
        }
        public User SetVehicle(int userID, Vehicle newVehicle)
        {
            var user = ParkingContext.Users.Include(u => u.Vehicle).FirstOrDefault(u => u.Id == userID);
            if (user == null || user.Vehicle != null)
                return null;
            user.Vehicle = newVehicle;
            return user;
        }
        public User RemoveVehicle(int userID)
        {
            var user = ParkingContext.Users.Include(u => u.Vehicle).FirstOrDefault(u => u.Id == userID);
            if (user == null || user.Vehicle == null)
                return null;
            user.Vehicle = null;
            return user;
        }
        public User UpdateUsername(int userID, string newUsername)
        {
            var user = ParkingContext.Users.Find(userID);
            user.Username = newUsername;
            return user;
        }
    }
}
