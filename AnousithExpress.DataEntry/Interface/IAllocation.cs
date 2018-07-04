using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IAllocation
    {
        List<ItemsSentAllocationModel> GetAll();
        List<ItemsSentAllocationModel> GetBySorting(int? customerId, int? routeId, int? timeId, DateTime? sendingDateFrom, DateTime? sendingDateTo);

        List<ItemsAllocationModelWithDelivery> GetAllocationForAdmin(int routeId, int timeId, DateTime? sendingDate);
        bool updateDeliveryman(int itemId, int DeliveryManId);
        List<ItemsPickUpAllocationModel> GetItemToPickUp(int customerId);
        bool AllocatePersonToPickUp(int itemId, int DeliveryManId);
    }
}
