using AnousithExpress.DataEntry.ViewModels.Customer;

namespace AnousithExpress.DataEntry.Interface
{
    public interface ICustomers
    {
        //Customer
        bool Register(ProfileModel model);
        bool Update(ProfileModel model);
        CustomerProfileItemsModel GetCustomerProfileItems(int customerId);
        ProfileModel GetCustomerProfile(int customerId);

    }
}
