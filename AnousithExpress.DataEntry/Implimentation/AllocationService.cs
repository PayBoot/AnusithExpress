using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
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

        public bool AllocatePersonToPickUp(int itemId, int DeliveryManId)
        {
            using (var db = new EntityContext())
            {
                if (!db.tbItemPickUpAllocations.Any(a => a.Item.Id == itemId))
                {
                    TbItemsPickUpAllocation itemAllocation = new TbItemsPickUpAllocation
                    {
                        DateToPickUp = DateTime.Now.Date,
                        DeliveryMan = db.tbUsers.FirstOrDefault(u => u.Id == DeliveryManId),
                        Item = db.TbItems.FirstOrDefault(i => i.Id == itemId)
                    };
                    db.tbItemPickUpAllocations.Add(itemAllocation);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    var itemAllocation = _item.GetAllPickUpAllocation(db).FirstOrDefault(a => a.Item.Id == itemId);
                    db.Entry(itemAllocation).State = System.Data.Entity.EntityState.Modified;
                    itemAllocation.DateToPickUp = DateTime.Now.Date;
                    itemAllocation.DeliveryMan = db.tbUsers.FirstOrDefault(u => u.Id == DeliveryManId);
                    db.SaveChanges();
                    return true;
                }

            }
        }

        public List<ItemsSentAllocationModel> GetAll()
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
                    .Where(x => x.Route.Id == routeId && x.Time.Id == timeId && x.DateToDeliver == sendingDate
                    && (x.Item.Status.Id != 6 || x.Item.Status.Id != 8)).ToList();
                List<ItemsAllocationModelWithDelivery> model = new List<ItemsAllocationModelWithDelivery>();
                model = source.Select(r => new ItemsAllocationModelWithDelivery
                {
                    Id = r.Id,
                    DateToDeliver = r.DateToDeliver.ToString("dd/MM/yyyy"),
                    DeliveryMan = r.DeliveryMan == null ? "" : r.DeliveryMan.Username,
                    ItemId = r.Item.Id,
                    ItemName = r.Item.ItemName,
                    ItemStatus = r.Item.Status.Status,
                    StatusId = r.Item.Status.Id,
                    Trackingnumber = r.Item.TrackingNumber,
                    TimeId = r.Time.Id,
                    TimeName = r.Time.Time,
                    RouteId = r.Route.Id,
                    RouteName = r.Route.Route
                }).ToList();
                return model;
            }
        }

        public List<ItemsSentAllocationModel> GetBySorting(int? customerId, int? routeId, int? timeId, DateTime? sendingDateFrom, DateTime? sendingDateTo)
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

        public List<ItemsPickUpAllocationModel> GetItemToPickUp(int customerId)
        {
            using (var db = new EntityContext())
            {
                var result = from item in _item.GetAll(db).Where(i => i.Customer.Id == customerId && i.Status.Id == 2).ToList()
                             from allocation in _item.GetAllPickUpAllocation(db).Where(a => a.Item.Customer.Id == customerId && a.Item.Status.Id == 2 && a.Item.Id == item.Id).ToList().DefaultIfEmpty()
                             select new ItemsPickUpAllocationModel
                             {
                                 TrackingNumber = item.TrackingNumber,
                                 CustomerAddress = item.Customer.Address,
                                 CustomerId = item.Customer.Id,
                                 CustomerName = item.Customer.Name,
                                 Status = item.Status.Status,
                                 StatusId = item.Status.Id,
                                 CustomerPhonenumber = item.Customer.Phonenumber,
                                 DateToDeliver = allocation == null ? "" : allocation.DateToPickUp.ToString("dd/MM/yyyy"),
                                 ItemId = item.Id,
                                 ItemName = item.ItemName,
                                 UserName = allocation == null ? "" : allocation.DeliveryMan.Username,
                                 Id = allocation == null ? 0 : allocation.Id
                             };

                return result.ToList();
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
