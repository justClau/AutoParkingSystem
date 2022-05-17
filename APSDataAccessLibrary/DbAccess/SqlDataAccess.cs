using APSDataAccessLibrary.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APSDataAccessLibrary.DbAccess
{
    public class SqlDataAccess
    {
        private readonly ParkingContext database;

        public SqlDataAccess(ParkingContext database)
        {
            this.database = database;
        }


    }
}
