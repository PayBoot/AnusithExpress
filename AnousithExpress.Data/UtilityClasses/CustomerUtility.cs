using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.UtilityClasses
{
    public class CustomerUtility
    {
        public CustomerSingleModel CustomerSingleModelProperty(TbCustomer customer)
        {
            return new CustomerSingleModel
            {
                Id = customer.Id,
                Address = customer.Address,
                BCEL_Baht = customer.BCEL_Baht,
                BCEL_Dollar = customer.BCEL_Dollar,
                BCEL_Kip = customer.BCEL_Kip,
                Phonenumber = customer.Phonenumber,
                Status = customer.Status.Status,
                Password = customer.Password,
                isDeleted = customer.isDeleted
            };
        }
        public TbCustomer GetCustomerById(EntityContext db, int custId)
        {
            return db.tbCustomers
                .Include(c => c.Status)
                .FirstOrDefault(c => c.Id == custId && c.isDeleted == false);
        }
        public IQueryable<TbCustomer> GetAllCustomer(EntityContext db)
        {
            return db.tbCustomers
                .Include(c => c.Status)
                .Where(c => c.isDeleted == false);
        }
    }
}
