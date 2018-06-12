using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using AnousithExpress.Data.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.Implementation
{
    public class CustService : ICustomer
    {
        private CustomerUtility customerUtility;
        private ItemsUtility itemsUtility;
        public CustService(CustomerUtility custUtility, ItemsUtility itemUtility)
        {
            customerUtility = custUtility;
            itemsUtility = itemUtility;
        }
        public bool CheckDuplicateNumber(string number)
        {
            using (var db = new EntityContext())
            {
                return db.tbCustomers.Any(c => c.Phonenumber == number);
            }
        }

        public bool CheckDuplicateNumber(int id, string number)
        {
            using (var db = new EntityContext())
            {
                var AllList = db.tbCustomers.Where(c => c.Id != id);
                return AllList.Any(a => a.Phonenumber == number);
            }
        }

        public bool Create(CustomerSingleModel model)
        {
            using (var db = new EntityContext())
            {
                if (!CheckDuplicateNumber(model.Phonenumber))
                {
                    TbCustomer customer = new TbCustomer
                    {
                        Phonenumber = model.Phonenumber,
                        Password = model.Password,
                        Address = model.Address,
                        BCEL_Baht = model.BCEL_Baht,
                        BCEL_Dollar = model.BCEL_Dollar,
                        BCEL_Kip = model.BCEL_Kip,
                        Status = db.tbStatuses.FirstOrDefault(s => s.Id == 1),
                        isDeleted = false
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

        public CustomerProfileModel CustomerProfile(int CustId)
        {
            using (var db = new EntityContext())
            {
                TbCustomer customer = customerUtility.GetCustomerById(db, CustId);
                CustomerSingleModel customerModelSource = customerUtility.CustomerSingleModelProperty(customer);
                List<TbItems> items = db.TbItems
                    .Include(i => i.Customer)
                    .Include(i => i.Status)
                    .Where(i => i.Customer.Id == CustId).ToList();
                List<ItemSingleModel> itemsModelSource = new List<ItemSingleModel>();
                if (items != null)
                {
                    itemsModelSource = itemsUtility.ItemListModelProperty(items);
                };

                CustomerProfileModel profile = new CustomerProfileModel
                {
                    customerModel = customerModelSource,
                    itemsModel = itemsModelSource
                };
                return profile;
            }
        }



        public bool Delete(int id)
        {
            using (var db = new EntityContext())
            {
                var customer = db.tbCustomers
                    .FirstOrDefault(c => c.Id == id);
                if (customer == null)
                {
                    return false;
                };
                db.Entry(customer).State = EntityState.Modified;
                customer.isDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool Edit(CustomerSingleModel model)
        {
            using (var db = new EntityContext())
            {
                int customerId = model.Id ?? default(int);
                if (CheckDuplicateNumber(customerId, model.Phonenumber))
                {
                    return false;
                };
                var customer = customerUtility.GetCustomerById(db, customerId);
                db.Entry(customer).State = EntityState.Modified;
                customer.Address = model.Address;
                customer.Phonenumber = model.Phonenumber;
                customer.Password = model.Password;
                customer.BCEL_Baht = model.BCEL_Baht;
                customer.BCEL_Dollar = model.BCEL_Dollar;
                customer.BCEL_Kip = model.BCEL_Kip;
                db.SaveChanges();
                return true;
            }
        }

        public List<CustomerSingleModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var customerList = customerUtility.GetAllCustomer(db);
                if (customerList != null)
                {
                    List<CustomerSingleModel> customerModel = customerList.Select(r => new CustomerSingleModel
                    {
                        Id = r.Id,
                        Status = r.Status.Status,
                        Address = r.Address,
                        BCEL_Baht = r.BCEL_Baht,
                        BCEL_Dollar = r.BCEL_Dollar,
                        BCEL_Kip = r.BCEL_Kip,
                        isDeleted = r.isDeleted,
                        Password = r.Password,
                        Phonenumber = r.Phonenumber
                    }).ToList();
                    return customerModel;
                }
                else
                {
                    return new List<CustomerSingleModel>();
                }
            }
        }



        public CustomerSingleModel GetSingle(int id)
        {
            using (var db = new EntityContext())
            {
                var customer = customerUtility.GetCustomerById(db, id);
                CustomerSingleModel model = customerUtility.CustomerSingleModelProperty(customer);
                return model;
            }
        }



        public TbCustomer LogIn(string phonenumber, string password)
        {
            using (var db = new EntityContext())
            {
                var customer = db.tbCustomers
                    .FirstOrDefault(c => c.Phonenumber == phonenumber && c.Password == password);
                return customer;
            }
        }

        public List<CustomerItemModel> ShowCustomer()
        {
            throw new NotImplementedException();
        }
    }
}
