using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.UtilityClasses
{
    public class ItemsUtility
    {
        public List<ItemSingleModel> AssingItemsProperty(List<TbItems> items)
        {
            return items.Select(r => new ItemSingleModel
            {
                Id = r.Id,
                TrackingNumber = r.TrackingNumber,
                CustomerId = r.Customer.Id,
                ItemName = r.ItemName,
                Status = r.Status.Status,
                ItemValue_Baht = r.ItemValue_Baht,
                ItemValue_Dollar = r.ItemValue_Dollar,
                ItemValue_Kip = r.ItemValue_Kip,
                CustomerPhonenumber = r.Customer.Phonenumber,
                Description = r.Descripttion,
                ReceiverName = r.ReceiverName,
                ReceipverPhone = r.ReceipverPhone,
                ReceiverAddress = r.ReceiverAddress,
                ReceiveDate = r.ReceiveDate == null ? "" : r.ReceiveDate?.ToString("dd-MM-yyyy"),
                ConfrimDate = r.ConfrimDate == null ? "" : r.ConfrimDate?.ToString("dd-MM-yyyy"),
                CreatedDate = r.CreatedDate == null ? "" : r.CreatedDate?.ToString("dd-MM-yyyy"),
                SendingDate = r.SendingDate == null ? "" : r.SendingDate?.ToString("dd-MM-yyyy"),
                isDeleted = r.isDeleted
            }).ToList();
        }
        public List<ItemSingleModel> AssignItemsProperty(List<TbItemAllocation> items)
        {
            return items.Select(r => new ItemSingleModel
            {
                Id = r.Id,
                TrackingNumber = r.Item.TrackingNumber,
                CustomerId = r.Item.Customer.Id,
                ItemName = r.Item.ItemName,
                Status = r.Item.Status.Status,
                ItemValue_Baht = r.Item.ItemValue_Baht,
                ItemValue_Dollar = r.Item.ItemValue_Dollar,
                ItemValue_Kip = r.Item.ItemValue_Kip,
                CustomerPhonenumber = r.Item.Customer.Phonenumber,
                Description = r.Item.Descripttion,
                ReceiverName = r.Item.ReceiverName,
                ReceipverPhone = r.Item.ReceipverPhone,
                ReceiverAddress = r.Item.ReceiverAddress,
                ReceiveDate = r.Item.ReceiveDate == null ? "" : r.Item.ReceiveDate?.ToString("dd-MM-yyyy"),
                ConfrimDate = r.Item.ConfrimDate == null ? "" : r.Item.ConfrimDate?.ToString("dd-MM-yyyy"),
                CreatedDate = r.Item.CreatedDate == null ? "" : r.Item.CreatedDate?.ToString("dd-MM-yyyy"),
                SendingDate = r.Item.SendingDate == null ? "" : r.Item.SendingDate?.ToString("dd-MM-yyyy"),
                isDeleted = r.Item.isDeleted
            }).ToList();
        }
        public TbItems GetItemById(int id, EntityContext db)
        {
            return db.TbItems
                .Include(i => i.Status)
                .Include(i => i.Customer)
                .FirstOrDefault(i => i.Id == id && i.isDeleted == false);
        }
        public IQueryable<TbItems> GetAllItem(EntityContext db)
        {
            return db.TbItems
                .Include(i => i.Status)
                .Include(i => i.Customer)
                .Where(i => i.isDeleted == false);
        }
        public IQueryable<TbItemAllocation> GetAllItemAllocation(EntityContext db)
        {
            return db.tbItemAllocations
                .Include(i => i.Route)
                .Include(i => i.Time)
                .Include(i => i.Item).Include(i => i.Item.Status).Include(i => i.Item.Customer)
                .Where(i => i.Item.isDeleted == false);
        }
    }
}
