using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface ICustomers
    {
        //Customer
        bool Register(ProfileModel model);
        bool Update(ProfileModel model);
        bool CheckExistingPhonenumber(string phonenumber, int? userId);
        CustomerProfileItemsModel GetCustomerProfileItems(int customerId);
        ProfileModel GetCustomerProfile(int customerId);

        List<CustomerModel> CustomerList();
        TbCustomer Login(string phonenumber, string password);
        int GetCustomerId(string phonenumber);
        int CheckCustomerId(string Id);
        
        
    }
}
