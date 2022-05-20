using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParkingContext context;
        private readonly IConfiguration config;

        public UnitOfWork(ParkingContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
            ParkingLots = new ParkingLotsRepository(context);
            Vehicles = new VehicleRepository(context);
            Users = new UsersRepository(context);
            Bills = new BillsRepository(context);
        }
        public IParkingLotsRepository ParkingLots { get; private set; }
        public IVehicleRepository Vehicles { get; private set; }
        public IBillsRepository Bills { get; private set; }
        public IUsersRepository Users { get; private set; }

        public int Commit() => context.SaveChanges();
        public void Dispose() => context.Dispose();

        public ConfigDTO ConfigParkingLot()
        {
            var configs = new DbConfig(config, context);
            return configs.StartParkingLotConfiguration();
        }

    }
}
