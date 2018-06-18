using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class CustomerService : ICustomers
    {
        private CustomerUtility _customer = new CustomerUtility();
        private ItemUtility _item = new ItemUtility();



        /// <summary>
        /// For Customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ProfileModel GetCustomerProfile(int customerId)
        {
            using (var db = new EntityContext())
            {
                var source = _customer.GetSingle(db, customerId);
                var result = _customer.AssignCustomerProfile(source);
                return result;
            }
        }

        public CustomerProfileItemsModel GetCustomerProfileItems(int customerId)
        {
            using (var db = new EntityContext())
            {
                var sourceCustomer = _customer.GetSingle(db, customerId);
                var sourceItem = _item.GetAll(db).Where(i => i.Customer.Id == customerId).ToList();
                CustomerProfileItemsModel result = new CustomerProfileItemsModel
                {
                    Profile = _customer.AssignCustomerProfile(sourceCustomer),
                    Items = _item.AssignItemsList(sourceItem)
                };
                return result;
            }
        }

        public bool Register(ProfileModel model)
        {
            using (var db = new EntityContext())
            {
                if (!_customer.CheckExistingPhonenumber(db, model.Phonenumber, 0))
                {
                    TbCustomer customer = new TbCustomer
                    {
                        Name = model.Name,
                        Status = db.tbStatuses.FirstOrDefault(s => s.Id == 1),
                        Address = model.Address,
                        BCEL_Baht = model.BCEL_Baht,
                        BCEL_Dollar = model.BCEL_Dollar,
                        BCEL_Kip = model.BCEL_Kip,
                        Phonenumber = model.Phonenumber,
                        isDeleted = false,
                        Password = model.Password
                    };
                    db.tbCustomers.Add(customer);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Update(ProfileModel model)
        {
            using (var db = new EntityContext())
            {
                if (!_customer.CheckExistingPhonenumber(db, model.Phonenumber, model.Id))
                {
                    var customer = _customer.GetSingle(db, (int)model.Id);
                    db.Entry(customer).State = EntityState.Modified;
                    customer.Name = model.Name;
                    customer.Password = model.Password;
                    customer.Phonenumber = model.Phonenumber;
                    customer.Address = model.Address;
                    customer.BCEL_Baht = model.BCEL_Baht;
                    customer.BCEL_Dollar = model.BCEL_Dollar;
                    customer.BCEL_Kip = model.BCEL_Kip;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
