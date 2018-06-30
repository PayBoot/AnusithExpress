using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class ConsolidationService : IConsolidation
    {
        private ConsolidateUtility _consolidate = new ConsolidateUtility();
        private ItemUtility _item = new ItemUtility();

        public bool CreateConsolidation(int cusomterId, double amount, double fee, int[] itemId)
        {
            using (var db = new EntityContext())
            {

                using (var dbtransact = db.Database.BeginTransaction())
                {
                    try
                    {
                        TbConsolidateList list = new TbConsolidateList
                        {
                            Customer = db.tbCustomers.FirstOrDefault(u => u.Id == cusomterId),
                            AmountOfItem = amount,
                            ConsolidatedDate = DateTime.Now.Date,
                            Fee = fee
                        };
                        db.tbConsolidateLists.Add(list);

                        db.SaveChanges();
                        db.Entry(list).State = System.Data.Entity.EntityState.Modified;
                        list.ConsolidateNumber = "L" + list.Id.ToString().PadLeft(5, '0');
                        foreach (var id in itemId)
                        {
                            TbConsolidatedItems itemlist = new TbConsolidatedItems
                            {
                                Consolidator = list,
                                Items = db.TbItems.FirstOrDefault(i => i.Id == id)
                            };
                            db.tbConsolidatedItems.Add(itemlist);
                        }
                        db.SaveChanges();
                        dbtransact.Commit();
                        return true;
                    }
                    catch (Exception)
                    {

                        dbtransact.Rollback();
                        return false;
                    }

                }


            }


        }

        public bool deleteConsolidation(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var list = _consolidate.GetAll(db).FirstOrDefault(c => c.Id == consolidationId);
                if (list == null)
                {
                    return false;
                };
                var items = _consolidate.GetAllItems(db).Where(i => i.Consolidator.Id == consolidationId);
                db.tbConsolidatedItems.RemoveRange(items);
                db.tbConsolidateLists.Remove(list);
                db.SaveChanges();
                return true;

            }
        }

        public List<ConsolidationListModel> GetAllConsolidationByDate(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var source = _consolidate.GetAll(db).ToList();
                if (fromDate != null)
                {
                    source = source.Where(s => s.ConsolidatedDate >= fromDate).ToList();
                }
                if (toDate != null)
                {
                    source = source.Where(s => s.ConsolidatedDate <= toDate).ToList();
                }
                if (source != null)
                {
                    var result = _consolidate.AssignConsolidationList(source);
                    ConsolidationListModel sumRow = new ConsolidationListModel
                    {
                        Id = 0,
                        AmountOfItem = result.Sum(x => x.AmountOfItem),
                        ConsolidatedDate = null,
                        CustomerID = 0,
                        ConsolidateNumber = null,
                        CustomerName = "ລວມ",
                        CustomerPhonenumber = null,
                        Fee = result.Sum(x => x.Fee)
                    };
                    result.Add(sumRow);

                    return result;
                }
                else
                {
                    return new List<ConsolidationListModel>();
                }
            }
        }

        public ConsolidationModel GetConsolidationDetailByConsolidationId(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var sourceDetail = _consolidate.GetSingle(db, consolidationId);
                var sourceItems = _consolidate.GetAllItems(db).Where(c => c.Consolidator.Id == consolidationId).ToList();
                ConsolidationModel result = new ConsolidationModel
                {
                    Detail = _consolidate.AssignSingleConsolidationList(sourceDetail),
                    Items = _consolidate.AssignConsolidationItemsList(sourceItems)
                };
                return result;

            }
        }

        public List<ConsolidationListModel> GetConsolidationListByCustomerId(int customerId, DateTime? searchFrom, DateTime? searchTo)
        {
            using (var db = new EntityContext())
            {
                var source = _consolidate.GetAll(db).Where(c => c.Customer.Id == customerId).ToList();
                if (searchFrom != null)
                {
                    source = source.Where(c => c.ConsolidatedDate >= searchFrom).ToList();
                }
                if (searchTo != null)
                {
                    source = source.Where(c => c.ConsolidatedDate <= searchTo).ToList();
                }
                List<ConsolidationListModel> result = _consolidate.AssignConsolidationList(source);
                return result;
            }
        }

        public double GetPrice(double condition)
        {
            using (var db = new EntityContext())
            {
                double fee = 0;
                List<TbPrice> priceCondition = new List<TbPrice>();
                priceCondition = db.tbPrices.ToList();
                List<double> con = new List<double>();
                List<double> price = new List<double>();
                foreach (var p in priceCondition)
                {
                    con.Add(p.Condition);
                    price.Add(p.PriceSet);
                }
                double[] conNumber = con.ToArray();
                double[] priceNumber = price.ToArray();
                for (int i = 0; i < conNumber.Length; i++)
                {
                    if (condition >= conNumber[i])
                    {
                        fee = priceNumber[i];
                    }
                }
                return fee;
            }
        }

        public List<ItemsModel> GetUnConlidateItems(int customerId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == customerId && i.Status.Id == 6
                        && !db.tbConsolidatedItems.Select(x => x.Items.Id).Contains(i.Id)).ToList();
                if (source != null)
                {
                    List<ItemsModel> result = _item.AssignItemsList(source.ToList());
                    return result;
                }
                else
                {
                    return new List<ItemsModel>();
                }
            }
        }


    }
}
