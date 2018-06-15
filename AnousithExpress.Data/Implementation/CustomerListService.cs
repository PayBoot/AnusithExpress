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

                List<CustomerItemModel> model = new List<CustomerItemModel>();
                foreach (var customer in customers)
                {
                    CustomerItemModel ci = new CustomerItemModel();
                    ci.CustomerId = customer.Id;
                    ci.CustomerName = customer.Name;
                    ci.CountItems = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id);
                    ci.CountItemsToPickup = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 2);
                    ci.CountItemsPickedUp = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 3);
                    ci.CountItemsInProcess = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 4);
                    ci.CountItemsToSend = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 5);
                    ci.CountItemsAlreadySend = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 6);
                    ci.CountItemsCannotContact = itemsUtility.GetAllItem(db).Count(i => i.Customer.Id == customer.Id && i.Status.Id == 7);
                    model.Add(ci);
                }
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
