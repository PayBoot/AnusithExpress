using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using AnousithExpress.Data.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AnousithExpress.Data.Implementation
{
    public class ConsolidatedService : IConsolidation
    {
        private ItemsUtility _itemUtility;
        public ConsolidatedService(ItemsUtility itemsUtility)
        {
            _itemUtility = itemsUtility;
        }
        public bool Create(ConsolidateSingleModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    TbConsolidateList consolidation = new TbConsolidateList
                    {
                        ConsolidatedDate = DateTime.Now,
                        AmountOfItem = model.ItemId.Length,
                        Customer = db.tbCustomers.FirstOrDefault(x => x.Id == model.CustomerId),
                        Fee = model.Fee,

                    };

                    db.tbConsolidateLists.Add(consolidation);
                    db.SaveChanges();
                    db.Entry(consolidation).State = EntityState.Modified;
                    consolidation.ConsolidateNumber = consolidation.Id.ToString().PadRight(6, '0');
                    db.SaveChanges();
                    foreach (var item in model.ItemId)
                    {
                        TbConsolidatedItems it = new TbConsolidatedItems
                        {
                            Consolidator = consolidation,
                            Items = db.TbItems.FirstOrDefault(x => x.Id == item)
                        };
                        db.tbConsolidatedItems.Add(it);
                    }
                    db.SaveChanges();
                    dbtransact.Commit();
                    return true;
                }

            }
        }

        public bool Delete(int id)
        {
            using (var db = new EntityContext())
            {
                var consolidate = db.tbConsolidateLists.FirstOrDefault(x => x.Id == id);
                var consolidatedItems = db.tbConsolidatedItems
                    .Include(x => x.Consolidator)
                    .Where(x => x.Consolidator.Id == id);
                if (consolidate == null)
                {
                    return false;
                };
                db.tbConsolidatedItems.RemoveRange(consolidatedItems);
                db.tbConsolidateLists.Remove(consolidate);

                db.SaveChanges();
                return true;
            }
        }

        public List<ConsolidateSingleModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.Include(x => x.Customer);
                List<ConsolidateSingleModel> list = new List<ConsolidateSingleModel>();
                list = source.Select(r => new ConsolidateSingleModel
                {
                    Id = r.Id,
                    Amount = r.AmountOfItem,
                    ConsolidateDate = r.ConsolidatedDate.ToString("dd-MM-yyyy"),
                    ConsolidateNumber = r.ConsolidateNumber,
                    CustomerId = r.Customer.Id,
                    CustomerName = r.Customer.Name,
                    CustomerPhonenumber = r.Customer.Phonenumber,
                    Fee = r.Fee,
                }).ToList();
                return list;
            }
        }

        public ConsolidatedItemsModel GetItems(int consolidatedId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists
                    .Include(x => x.Customer)
                    .FirstOrDefault(x => x.Id == consolidatedId);
                var itemsource = _itemUtility.GetAllItem(db).Where(x => db.tbConsolidatedItems.Where(c => c.Consolidator.Id == consolidatedId).Select(c => c.Items.Id).Contains(x.Id)).ToList();
                ConsolidatedItemsModel model = new ConsolidatedItemsModel
                {
                    ConsolidationId = source.Id,
                    ConsolidationNumber = source.ConsolidateNumber,
                    Customername = source.Customer.Name,
                    CustomerPhonenumber = source.Customer.Phonenumber,
                    Items = _itemUtility.AssingItemsProperty(itemsource)
                };
                return model;
            }
        }

        public List<ConsolidateSingleModel> GetSingle(int customerId, DateTime? dateFrom, DateTime? dateTo)
        {
            using (var db = new EntityContext())
            {
                List<TbConsolidateList> source = new List<TbConsolidateList>();
                if (dateFrom == null && dateTo == null)
                {
                    source = db.tbConsolidateLists.Include(x => x.Customer)
                   .Where(x => x.Customer.Id == customerId).ToList();
                }
                else
                {
                    source = db.tbConsolidateLists.Include(x => x.Customer)
                    .Where(x => x.Customer.Id == customerId && x.ConsolidatedDate >= dateFrom && x.ConsolidatedDate <= dateTo).ToList();
                }
                List<ConsolidateSingleModel> list = new List<ConsolidateSingleModel>();
                list = source.Select(r => new ConsolidateSingleModel
                {
                    Id = r.Id,
                    Amount = r.AmountOfItem,
                    ConsolidateDate = r.ConsolidatedDate.ToString("dd-MM-yyyy"),
                    ConsolidateNumber = r.ConsolidateNumber,
                    CustomerId = r.Customer.Id,
                    CustomerName = r.Customer.Name,
                    CustomerPhonenumber = r.Customer.Phonenumber,
                    Fee = r.Fee,
                }).ToList();
                return list;
            }
        }
        public bool Update(ConsolidateSingleModel model)
        {
            throw new NotImplementedException();
        }
    }
}
