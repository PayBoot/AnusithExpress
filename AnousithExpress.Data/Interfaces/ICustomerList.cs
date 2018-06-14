using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;

namespace AnousithExpress.Data.Interfaces
{
    public interface ICustomerList
    {
        List<CustomerItemModel> GetCustomerList();
        List<CustomerNotification> GetNewItemsNotification();
        List<ItemSingleModel> GetItemsForCustomer(int CustId);
        List<ItemSingleModel> GetItemsByStatusForCustomer(int CustId, int statusId);
    }
}
