using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.DataEntry.Utility
{
    public class CustomerUtility
    {
        public ProfileModel AssignCustomerProfile(TbCustomer customer)
        {
            return new ProfileModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address,
                BCEL_Baht = customer.BCEL_Baht,
                BCEL_Dollar = customer.BCEL_Dollar,
                BCEL_Kip = customer.BCEL_Kip,
                Phonenumber = customer.Phonenumber,
                Password = customer.Password
            };
        }
        public TbCustomer GetSingle(EntityContext db, int custId)
        {
            return db.tbCustomers
                .Include(c => c.Status)
                .FirstOrDefault(c => c.Id == custId && c.isDeleted == false);
        }
        public IQueryable<TbCustomer> GetAll(EntityContext db)
        {
            return db.tbCustomers
                .Include(c => c.Status)
                .Where(c => c.isDeleted == false);
        }

        public bool CheckExistingPhonenumber(EntityContext db, string phonenumber, int? id)
        {
            if (id > 0)
            {
                return db.tbCustomers.Where(c => c.Id != id).Any(c => c.Phonenumber == phonenumber);
            }
            else
            {
                return db.tbCustomers.Any(c => c.Phonenumber == phonenumber);
            }

        }
    }
}
