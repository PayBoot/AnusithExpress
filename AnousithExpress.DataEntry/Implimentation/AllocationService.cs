using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Admin;
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

        public List<ItemsAllocationModelWithDelivery> GetAllocationForAdmin(int routeId, int timeId, DateTime? sendingDate)
        {
            using (var db = new EntityContext())
            {

                var source = _item.GetAllAllocation(db)
                    .Where(x => x.Route.Id == routeId && x.Time.Id == timeId && x.DateToDeliver == sendingDate).ToList();
                List<ItemsAllocationModelWithDelivery> model = new List<ItemsAllocationModelWithDelivery>();
                model = source.Select(r => new ItemsAllocationModelWithDelivery
                {
                    Id = r.Id,
                    DateToDeliver = r.DateToDeliver.ToString("dd/MM/yyyy"),
                    DeliveryMan = r.DeliveryMan == null ? "" : r.DeliveryMan.Username,
                    ItemId = r.Item.Id,
                    ItemName = r.Item.ItemName,
                    ItemStatus = r.Item.Status.Status,
                    Trackingnumber = r.Item.TrackingNumber,
                    TimeId = r.Time.Id,
                    TimeName = r.Time.Time,
                    RouteId = r.Route.Id,
                    RouteName = r.Route.Route
                }).ToList();
                return model;
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

        public bool updateDeliveryman(int itemId, int DeliveryManId)
        {
            using (var db = new EntityContext())
            {

                var item = _item.GetAllAllocation(db).FirstOrDefault(x => x.Item.Id == itemId);
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                item.DeliveryMan = db.tbUsers.FirstOrDefault(x => x.Id == DeliveryManId);
                db.SaveChanges();
                return true;

            }
        }
    }
}
