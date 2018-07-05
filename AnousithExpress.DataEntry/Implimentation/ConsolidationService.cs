using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                        //get balance from delivery
                        double kip = 0;
                        double baht = 0;
                        double dollar = 0;
                        foreach (var id in itemId)
                        {
                            kip += db.tbItemSentHistories
                                .Include(x => x.Item)
                                .FirstOrDefault(x => x.Item.Id == id).IncomingBalanceInKip;
                            baht += db.tbItemSentHistories
                                 .Include(x => x.Item)
                                .FirstOrDefault(x => x.Item.Id == id).IncomingBalanceInBaht;
                            dollar += db.tbItemSentHistories.Include(x => x.Item)
                                .FirstOrDefault(x => x.Item.Id == id).IncomingBalanceInDollar;
                        }

                        double balanceToTransfer = kip - fee;
                        TbConsolidateList list = new TbConsolidateList
                        {
                            Customer = db.tbCustomers.FirstOrDefault(u => u.Id == cusomterId),
                            AmountOfItem = amount,
                            ConsolidatedDate = DateTime.Now.Date,
                            IncomingBalanceInKip = kip,
                            IncomingBalanceInBaht = baht,
                            IncomingBalanceInDollar = dollar,
                            BalanceToTransfer = balanceToTransfer,
                            isBalanceTransfer = false,
                            isCustomerConfirmed = false,
                            Fee = fee,

                        };
                        db.tbConsolidateLists.Add(list);

                        db.SaveChanges();
                        db.Entry(list).State = EntityState.Modified;
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
                    ConsolidateDetail = _consolidate.AssignSingleConsolidationList(sourceDetail),
                    ConsolidationItems = AssignConsolidateItems(sourceItems)
                };
                return result;
            }
        }

        private static List<ItemsModel> AssignConsolidateItems(List<TbConsolidatedItems> sourceItems)
        {
            return sourceItems.Select(r => new ItemsModel
            {
                ItemName = r.Items.ItemName,
                TrackingNumber = r.Items.TrackingNumber,
                Status = r.Items.Status.Status,
                CustomerName = r.Items.Customer.Name,
                CustomerId = r.Items.Customer.Id,
                CustomerPhonenumber = r.Items.Customer.Phonenumber,
                Id = r.Items.Id,
                ConfrimDate = r.Items.ConfrimDate == null ? "" : r.Items.ConfrimDate?.ToString("dd-MM-yyyy"),
                CreatedDate = r.Items.CreatedDate == null ? "" : r.Items.CreatedDate?.ToString("dd-MM-yyyy"),
                Description = r.Items.Descripttion,
                SendingDate = r.Items.SendingDate == null ? "" : r.Items.SendingDate?.ToString("dd-MM-yyyy"),
                SentDate = r.Items.SentDate == null ? "" : r.Items.SentDate?.ToString("dd-MM-yyyy"),
                isDeleted = r.Items.isDeleted,
                ItemValue_Baht = r.Items.ItemValue_Baht,
                ItemValue_Dollar = r.Items.ItemValue_Dollar,
                ItemValue_Kip = r.Items.ItemValue_Kip,
                ReceipverPhone = r.Items.ReceipverPhone,
                ReceiveDate = r.Items.ReceiveDate == null ? "" : r.Items.ReceiveDate?.ToString("dd-MM-yyyy"),
                ReceiverAddress = r.Items.ReceiverAddress,
                ReceiverName = r.Items.ReceiverName
            }).ToList();
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

        public ConsolidateFactorModel GetConsolidationFactor(int[] itemId, int customerId)
        {
            using (var db = new EntityContext())
            {
                if (itemId.Length == 0)
                {
                    return null;
                };
                double condition = GetCondition(itemId, customerId, db);
                string Date = GetDate(itemId, customerId, db);
                if (condition == 0)
                {
                    return null;
                };
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
                return new ConsolidateFactorModel
                {
                    ConditionUsed = condition,
                    DateOfReceive = Date,
                    PricePerUnit = fee
                };
            }
        }
        private double GetCondition(int[] itemId, int customerId, EntityContext db)
        {
            bool valueEqual = false;

            List<DateTime> AllDate = new List<DateTime>();

            foreach (var id in itemId)
            {
                AllDate.Add((DateTime)db.TbItems
                    .Include(s => s.Customer).FirstOrDefault(s => s.Id == id
                        && s.Customer.Id == customerId).ReceiveDate);

            }
            DateTime[] DateString = AllDate.ToArray();
            for (int r = 0; r < DateString.Length; r++)
            {
                if (r >= 1)
                {
                    valueEqual = AllDate[0].ToString().Equals(AllDate[r].ToString(), StringComparison.InvariantCulture);
                    if (!valueEqual)
                    {
                        return 0;
                    }
                }
            }
            DateTime resultTime = new DateTime();
            foreach (var time in AllDate)
            {
                resultTime = time;
            }
            double count = db.TbItems
                .Include(s => s.Customer).Where(x => x.ReceiveDate == resultTime
                        && x.Customer.Id == customerId).Count();
            return count;


        }

        private string GetDate(int[] itemId, int customerId, EntityContext db)
        {
            bool valueEqual = false;

            List<DateTime> AllDate = new List<DateTime>();

            foreach (var id in itemId)
            {
                AllDate.Add((DateTime)db.TbItems.Include(s => s.Customer).FirstOrDefault(s => s.Id == id && s.Customer.Id == customerId).ReceiveDate);

            }
            DateTime[] DateString = AllDate.ToArray();
            for (int r = 0; r < DateString.Length; r++)
            {
                if (r >= 1)
                {
                    valueEqual = AllDate[0].ToString().Equals(AllDate[r].ToString(), StringComparison.InvariantCulture);
                    if (!valueEqual)
                    {
                        return "";
                    }
                }
            }
            DateTime resultTime = new DateTime();
            foreach (var time in AllDate)
            {
                resultTime = time;
            }
            return resultTime.ToString("dd-MM-yyyy");
        }

        public List<ItemsModel> GetUnConlidateItems(int customerId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == customerId && (i.Status.Id == 6 || i.Status.Id == 10)
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

        public bool AddNeedConfirm(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.needConfirm = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool UndoNeedConfirm(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.needConfirm = false;
                db.SaveChanges();
                return true;
            }
        }

        public bool CustomerConfirm(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.isCustomerConfirmed = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool UndoCustomerConfirm(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.isCustomerConfirmed = false;
                db.SaveChanges();
                return true;
            }
        }

        public bool ConfirmTransfer(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.isBalanceTransfer = true;
                db.SaveChanges();
                return true;
            }
        }

        public bool UndoConfirmTransfer(int consolidationId)
        {
            using (var db = new EntityContext())
            {
                var source = db.tbConsolidateLists.FirstOrDefault(c => c.Id == consolidationId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.isBalanceTransfer = false;
                db.SaveChanges();
                return true;
            }
        }

        public List<ConsolidationListModel> GetConsolidationNotification()
        {
            using (var db = new EntityContext())
            {
                var source = _consolidate.GetAll(db).Where(x => x.needConfirm == true && x.isCustomerConfirmed == true && x.isBalanceTransfer == false);
                var result = _consolidate.AssignConsolidationList(source.ToList());
                return result;
            }
        }

        public double CountConfirmConsolidation()
        {
            using (var db = new EntityContext())
            {
                double number = _consolidate.GetAll(db).Where(x => x.needConfirm == true && x.isCustomerConfirmed == true && x.isBalanceTransfer == false).Count();

                return number;
            }
        }
    }
}
