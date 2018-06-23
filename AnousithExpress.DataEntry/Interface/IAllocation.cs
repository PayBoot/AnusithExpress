using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IAllocation
    {
        List<ItemsAllocationModel> GetAll();
        List<ItemsAllocationModel> GetBySorting(int? customerId, int? routeId, int? timeId, DateTime? sendingDateFrom, DateTime? sendingDateTo);
    }
}
