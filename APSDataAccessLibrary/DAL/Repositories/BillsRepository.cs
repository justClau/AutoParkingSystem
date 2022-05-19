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
    public class BillsRepository : Repository<Bill>, IBillsRepository
    {
        //private readonly ParkingContext context;
        public ParkingContext ParkingContext { get { return context as ParkingContext; } }

        public BillsRepository(ParkingContext context)
            : base(context)
        {
        }
        public IEnumerable<Bill> GetUserBills(int UserID)
        {
            var query = from bill in ParkingContext.Bills.Include("User")
                        where bill.User.Id == UserID
                        orderby bill.BillValue descending
                        select bill;
            return query;
        }
        public IEnumerable<Bill> GetBills(int pageIndex, int pageSize = 10)
        {
            return ParkingContext.Bills
                .Include(b => b.User)
                .OrderBy(b => b.User.FullName)
                .ThenByDescending(b => b.BillValue)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
