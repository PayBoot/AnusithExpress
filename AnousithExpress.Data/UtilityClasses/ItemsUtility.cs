using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.UtilityClasses
{
    public class ItemsUtility
    {
        public List<ItemSingleModel> ItemListModelProperty(List<TbItems> items)
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
    }
}
