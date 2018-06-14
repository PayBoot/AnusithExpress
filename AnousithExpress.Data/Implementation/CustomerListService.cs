using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.SingleViewModels;
using AnousithExpress.Data.UtilityClasses;
using System.Collections.Generic;
using System.Linq;

namespace AnousithExpress.Data.Implementation
{
    public class CustomerListService : ICustomerList
    {
        private CustomerUtility customerUtility;
        private ItemsUtility itemsUtility;
        public CustomerListService(CustomerUtility _customerUtility, ItemsUtility _itemsUtility)
        {
            itemsUtility = _itemsUtility;
            customerUtility = _customerUtility;
        }
        public List<CustomerItemModel> GetCustomerList()
        {
            using (var db = new EntityContext())
            {
                var customers = customerUtility.GetAllCustomer(db);
                var model = customers.Select(r => new CustomerItemModel
                {
                    CustomerId = r.Id,
                    CustomerName = r.Name,
                    CustomerPhonenumber = r.Phonenumber,
                    CountItems = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id).Count(),
                    CouteItemsToPickup = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 2).Count(),
                    CountItemsPickedUp = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 3).Count(),
                    CouteItemsInProcess = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 4).Count(),
                    CountItemsToSend = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 5).Count(),
                    CountItemsAlreadySend = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 6).Count(),
                    CountItemsCannotContact = itemsUtility.GetAllItem(db)
                            .Where(x => x.Customer.Id == r.Id && x.Status.Id == 7).Count(),

                });
                return model.ToList();
            }
        }

        public List<ItemSingleModel> GetItemsByStatusForCustomer(int CustId, int statusId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db)
                    .Where(x => x.Customer.Id == CustId && x.Status.Id == statusId);
                var model = itemsUtility.ItemListModelProperty(items.ToList());
                return model;

            }
        }

        public List<ItemSingleModel> GetItemsForCustomer(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db)
                    .Where(x => x.Customer.Id == CustId);
                var model = itemsUtility.ItemListModelProperty(items.ToList());
                return model;
            }
        }

        public List<CustomerNotification> GetNewItemsNotification()
        {
            using (var db = new EntityContext())
            {

                return new List<CustomerNotification>();
            }
        }
    }
}
