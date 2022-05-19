using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public ParkingContext ParkingContext { get { return context as ParkingContext; } }
        public VehicleRepository(ParkingContext context) : base(context) { }


        public Vehicle GetVehicleByVIN(string VIN)
            => ParkingContext.Vehicles.Where(veh => veh.VIN == VIN).FirstOrDefault();

        public Vehicle GetVehicleByPlate(string Plate)
            => ParkingContext.Vehicles.Where(veh => veh.PlateNumber == Plate).FirstOrDefault();
    }
}
