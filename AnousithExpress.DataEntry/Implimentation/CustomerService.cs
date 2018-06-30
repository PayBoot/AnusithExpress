using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Collections.Generic;
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
                    if (model.Password != null)
                    {
                        customer.Password = model.Password;
                    };
                    if (!string.IsNullOrEmpty(model.Status))
                    {
                        customer.Status = db.tbStatuses.FirstOrDefault(s => s.Status == model.Status);
                    };

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

        public TbCustomer Login(string phonenumber, string password)
        {
            using (var db = new EntityContext())
            {
                var source = _customer.GetAll(db).Where(c => c.Phonenumber == phonenumber && c.Password == password).FirstOrDefault();
                return source;
            }
        }

        /// <summary>
        /// For Admin
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        public List<CustomerModel> CustomerList()
        {
            using (var db = new EntityContext())
            {
                var source = _customer.GetAll(db).ToList();
                List<CustomerModel> model = new List<CustomerModel>();
                foreach (var customer in source)
                {
                    var cust = new CustomerModel
                    {
                        CustomerId = customer.Id,
                        CustomerName = customer.Name,
                        NumberOfConfirmItem = _item.GetAll(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 2),
                        NumberOfInProcessItem = _item.GetAll(db).Count(i => i.Customer.Id == customer.Id && (i.Status.Id == 4 || i.Status.Id == 7)),
                        NumberOfProcessedItem = _item.GetAll(db).Count(i => i.Customer.Id == customer.Id && (i.Status.Id == 6 || i.Status.Id == 8)),
                        NumberOfItemSending = _item.GetAll(db).Count(i => i.Customer.Id == customer.Id && (i.Status.Id == 5 || i.Status.Id == 3 || i.Status.Id == 9))
                    };
                    model.Add(cust);
                }

                return model;

            }
        }

        public bool CheckExistingPhonenumber(string phonenumber, int? userId)
        {
            using (var db = new EntityContext())
            {
                return _customer.CheckExistingPhonenumber(db, phonenumber, userId);
            }
        }

        public int GetCustomerId(string phonenumber)
        {
            using (var db = new EntityContext())
            {
                var source = _customer.GetAll(db).FirstOrDefault(c => c.Phonenumber == phonenumber);
                if (source != null)
                {
                    return source.Id;
                }
                else
                {
                    return 0;
                }

            }
        }

        public int CheckCustomerId(string Id)
        {
            using (var db = new EntityContext())
            {
                int number;
                bool result = int.TryParse(Id, out number);
                if (result)
                {
                    if (_customer.GetAll(db).Any(c => c.Id == number))
                    {
                        return number;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
