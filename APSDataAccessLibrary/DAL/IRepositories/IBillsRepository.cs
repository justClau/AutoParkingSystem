using APSDataAccessLibrary.Context;
using APSDataAccessLibrary.Models;

namespace APSDataAccessLibrary.DAL.Repositories
{
    public interface IBillsRepository : IRepository<Bill>
    {
        IEnumerable<Bill> GetBills(int pageIndex, int pageSize = 10);
        IEnumerable<Bill> GetBillsByUserId(int userID);
    }
}