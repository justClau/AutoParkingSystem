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
    internal class ParkingLotsRepository : Repository<ParkingLot>, IParkingLotsRepository
    {
        public ParkingContext ParkingContext { get { return context as ParkingContext; } }
        public ParkingLotsRepository(ParkingContext context) : base(context) { }

        public ParkingLot GetByVehicle(int VehicleID)
            => ParkingContext.ParkingLots
            .Include(u => u.Vehicle)
            .Where(p => p.Vehicle.Id == VehicleID)
            .FirstOrDefault();

        public ParkingLot GetByName(int FloorNumber, string Name)
            => ParkingContext.ParkingLots
            .Include("Vehicle")
            .Where(veh => veh.Floor == FloorNumber && veh.Name.StartsWith(Name))
            .FirstOrDefault();

        public IEnumerable<ParkingLot> GetFree()
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(p => p.Vehicle == null)
            .ToList();

        public IEnumerable<ParkingLot> GetFreeOnFloor(int FloorNumber)
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(p => p.Floor == FloorNumber && p.Vehicle == null)
            .ToList();

        public ParkingLot SetVehicle(int ParkingLotID, Vehicle Vehicle)
        {
            var parkingLot = ParkingContext.ParkingLots.Find(ParkingLotID);
            ParkingContext.Entry(parkingLot).Reference(v => v.Vehicle).Load();

            if (parkingLot == null)
                return null;

            if (parkingLot.Vehicle != null)
                return null;

            parkingLot.Vehicle = Vehicle;
            return parkingLot;
        }

        public ParkingLot AddVehicle(Vehicle Vehicle)
        {
            var parkingLot = ParkingContext.ParkingLots
                .Include(p => p.Vehicle)
                .Where(p => p.Vehicle == null)
                .FirstOrDefault();

            if (parkingLot == null)
                return null;

            parkingLot.Vehicle = Vehicle;
            return parkingLot;
        }

        public ParkingLot RemoveVehicle(int VehicleID)
        {
            var parkingLot = ParkingContext.ParkingLots
                .Include(p => p.Vehicle)
                .Where(p => p.Vehicle.Id == VehicleID)
                .FirstOrDefault();
            parkingLot.Vehicle = null;
            return parkingLot;
        }
    }
}
