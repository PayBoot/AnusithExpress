using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.DataEntry.Utility
{
    public class ConsolidateUtility
    {
        public TbConsolidateList GetSingle(EntityContext db, int id)
        {
            return db.tbConsolidateLists.Include(c => c.Customer).FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<TbConsolidateList> GetAll(EntityContext db)
        {
            return db.tbConsolidateLists.Include(c => c.Customer);
        }

        public IQueryable<TbConsolidatedItems> GetAllItems(EntityContext db)
        {
            return db.tbConsolidatedItems
                .Include(c => c.Items)
                .Include(c => c.Items.Status)
                .Include(c => c.Items.Customer)
                .Include(c => c.Consolidator);
        }
        public List<ConsolidationListModel> AssignConsolidationList(List<TbConsolidateList> source)
        {
            if (source != null)
            {
                return source.Select(r => new ConsolidationListModel
                {
                    Id = r.Id,
                    AmountOfItem = r.AmountOfItem,
                    ConsolidatedDate = r.ConsolidatedDate.ToString("dd-MM-yyyy"),
                    ConsolidateNumber = r.ConsolidateNumber,
                    CustomerName = r.Customer.Name,
                    CustomerID = r.Customer.Id,
                    CustomerPhonenumber = r.Customer.Phonenumber,
                    Fee = r.Fee
                }).ToList();
            }
            else
            {
                return new List<ConsolidationListModel>();
            }
        }

        public ConsolidationListModel AssignSingleConsolidationList(TbConsolidateList source)
        {
            if (source != null)
            {
                return new ConsolidationListModel
                {
                    Id = source.Id,
                    AmountOfItem = source.AmountOfItem,
                    ConsolidatedDate = source.ConsolidatedDate.ToString("dd-MM-yyyy"),
                    ConsolidateNumber = source.ConsolidateNumber,
                    CustomerName = source.Customer.Name,
                    CustomerID = source.Customer.Id,
                    CustomerPhonenumber = source.Customer.Phonenumber,
                    Fee = source.Fee
                };
            }
            else
            {
                return null;
            }
        }
        public List<ConsolidationItemModel> AssignConsolidationItemsList(List<TbConsolidatedItems> consolidation)
        {
            if (consolidation != null)
            {
                return consolidation.Select(r => new ConsolidationItemModel
                {
                    Id = r.Id,
                    ConsolidationId = r.Consolidator.Id,
                    ItemsId = r.Items.Id,
                    ConsolidationNumber = r.Consolidator.ConsolidateNumber,
                    TrackingNumber = r.Items.TrackingNumber,
                    Status = r.Items.Status.Status,
                    CustomerId = r.Items.Customer.Id,
                    CustomerName = r.Items.Customer.Name,
                    CustomerPhonenumber = r.Items.Customer.Phonenumber,
                    Description = r.Items.Descripttion,
                    isDeleted = r.Items.isDeleted,
                    ItemName = r.Items.ItemName,
                    ItemValue_Baht = r.Items.ItemValue_Baht,
                    ItemValue_Dollar = r.Items.ItemValue_Dollar,
                    ItemValue_Kip = r.Items.ItemValue_Kip,
                    ConfrimDate = r.Items.ConfrimDate == null ? "" : r.Items.ConfrimDate?.ToString("dd-MM-yyyy"),
                    CreatedDate = r.Items.CreatedDate == null ? "" : r.Items.CreatedDate?.ToString("dd-MM-yyyy"),
                    ReceiveDate = r.Items.ReceiveDate == null ? "" : r.Items.ReceiveDate?.ToString("dd-MM-yyyy"),
                    SendingDate = r.Items.SendingDate == null ? "" : r.Items.SendingDate?.ToString("dd-MM-yyyy"),
                    SentDate = r.Items.SentDate == null ? "" : r.Items.SentDate?.ToString("dd-MM-yyyy"),
                    ReceiverAddress = r.Items.ReceiverAddress,
                    ReceiverName = r.Items.ReceiverName,
                    ReceiverPhone = r.Items.ReceipverPhone
                }).ToList();
            }
            else
            {
                return new List<ConsolidationItemModel>();
            }

        }
    }
}
