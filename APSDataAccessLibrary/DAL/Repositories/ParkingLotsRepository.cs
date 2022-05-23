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
    public class ParkingLotsRepository : Repository<ParkingLot>, IParkingLotsRepository
    {
        public ParkingContext ParkingContext { get { return context as ParkingContext; } }
        public ParkingLotsRepository(ParkingContext context) : base(context) { }

        public ParkingLot GetByVehicle(int vehicleID)
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(p => p.Vehicle.Id == vehicleID)
            .FirstOrDefault();

        public ParkingLot GetByName(int floorNumber, string name)
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(veh => veh.FloorNumber == floorNumber && veh.Name.StartsWith(name))
            .FirstOrDefault();

        public override IEnumerable<ParkingLot> GetAll()
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .ToList();

        public IEnumerable<ParkingLot> GetFree()
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(p => p.Vehicle == null)
            .ToList();

        public IEnumerable<ParkingLot> GetFreeOnFloor(int floorNumber)
            => ParkingContext.ParkingLots
            .Include(p => p.Vehicle)
            .Where(p => p.FloorNumber == floorNumber && p.Vehicle == null)
            .ToList();

        public ParkingLot SetVehicle(int parkingLotID, Vehicle vehicle)
        {
            var parkingLot = ParkingContext.ParkingLots.Find(parkingLotID);
            ParkingContext.Entry(parkingLot).Reference(v => v.Vehicle).Load();

            if (parkingLot == null)
                return null;

            if (parkingLot.Vehicle != null)
                return null;

            parkingLot.Vehicle = vehicle;
            return parkingLot;
        }

        public ParkingLot AddVehicle(Vehicle vehicle)
        {
            var parkingLot = ParkingContext.ParkingLots
                .Include(p => p.Vehicle)
                .Where(p => p.Vehicle == null)
                .FirstOrDefault();

            if (parkingLot == null)
                return null;

            parkingLot.Vehicle = vehicle;
            return parkingLot;
        }

        public ParkingLot RemoveVehicle(int vehicleID)
        {
            var parkingLot = ParkingContext.ParkingLots
                .Include(p => p.Vehicle)
                .Where(p => p.Vehicle.Id == vehicleID)
                .FirstOrDefault();
            parkingLot.Vehicle = null;
            return parkingLot;
        }

        public ParkingSchema GetParkingConfiguration()
        {
            return ParkingContext.ParkingSchema.FirstOrDefault();
        }
    }
}
