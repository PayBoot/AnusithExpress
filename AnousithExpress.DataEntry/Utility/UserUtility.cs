using AnousithExpress.DataEntry.Models;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.DataEntry.Utility
{
    public class UserUtility
    {
        public IQueryable<TbUser> GetAll(EntityContext db)
        {
            return db.tbUsers
                .Include(u => u.Role)
                .Include(u => u.Status)
                .Where(u => u.isDelete == false);
        }
    }
}
