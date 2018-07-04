using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Admin;
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
                    CustomerId = r.Customer.Id,
                    IncomingBalanceInBaht = r.IncomingBalanceInBaht,
                    IncomingBalanceInKip = r.IncomingBalanceInKip,
                    IncomingBalanceInDollar = r.IncomingBalanceInDollar,
                    isBalanceTransfer = r.isBalanceTransfer,
                    isCustomerConfirmed = r.isCustomerConfirmed,
                    needConfirm = r.needConfirm,
                    BalanceToTransferInKip = r.BalanceToTransfer,
                    CustomerPhoneNumber = r.Customer.Phonenumber,
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
                    CustomerId = source.Customer.Id,
                    CustomerPhoneNumber = source.Customer.Phonenumber,
                    IncomingBalanceInBaht = source.IncomingBalanceInBaht,
                    IncomingBalanceInKip = source.IncomingBalanceInKip,
                    IncomingBalanceInDollar = source.IncomingBalanceInDollar,
                    isBalanceTransfer = source.isBalanceTransfer,
                    isCustomerConfirmed = source.isCustomerConfirmed,
                    Fee = source.Fee,
                    BalanceToTransferInKip = source.BalanceToTransfer,
                    needConfirm = source.needConfirm
                };
            }
            else
            {
                return null;
            }
        }

    }
}
