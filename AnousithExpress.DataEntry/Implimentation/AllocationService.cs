using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class AllocationService : IAllocation
    {
        private ItemUtility _item = new ItemUtility();
        public List<ItemsAllocationModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAllAllocation(db).ToList();
                var result = _item.AssignItemsAllocation(source);
                return result;
            }
        }

        public List<ItemsAllocationModel> GetBySorting(int? customerId, int? routeId, int? timeId, DateTime? sendingDateFrom, DateTime? sendingDateTo)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAllAllocation(db)
                    .ToList();
                if (customerId != null)
                {
                    source = source.Where(i => i.Item.Customer.Id == customerId).ToList();
                }
                if (routeId != null)
                {
                    source = source.Where(i => i.Route.Id == routeId).ToList();
                }
                if (timeId != null)
                {
                    source = source.Where(i => i.Time.Id == timeId).ToList();
                }
                if (sendingDateFrom != null)
                {
                    source = source.Where(i => i.DateToDeliver >= sendingDateFrom).ToList();

                }
                if (sendingDateTo != null)
                {
                    source = source.Where(i => i.DateToDeliver <= sendingDateTo).ToList();
                }
                var result = _item.AssignItemsAllocation(source);
                return result;
            }
        }
    }
}
