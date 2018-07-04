using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class ProductService : IProducts
    {
        private ItemUtility _item = new ItemUtility();
        private CustomerUtility _customer = new CustomerUtility();

        /// <summary>
        /// For Customer Use Only
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Create(ItemsModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 1;
                    TbItems item = _item.CreateProduct(model, db, itemstatus);
                    dbtransact.Commit();
                    return item.Id;
                }
            }
        }

        public bool Delete(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                if (item == null)
                    return false;
                db.Entry(item).State = EntityState.Modified;
                item.isDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public List<ItemsModel> GetProductByCustomerId(int CustomerId, DateTime? CreateDate, DateTime? SubmitDate, string Status)
        {
            using (var db = new EntityContext())
            {

                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId).ToList();

                if (CreateDate != null)
                {
                    source = source.Where(i => i.CreatedDate == CreateDate).ToList();
                }
                if (SubmitDate != null)
                {
                    source = source.Where(i => i.ConfrimDate == SubmitDate).ToList();
                }
                if (!string.IsNullOrEmpty(Status))
                {
                    if (db.tbItemStatuses.Any(x => x.Status == Status))
                    {
                        source = source.Where(i => i.Status.Status == Status).ToList();
                    }
                }
                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }


            }
        }

        public ItemsModel GetSingle(int itemId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetSingle(db, itemId);
                var result = _item.AssignItem(source);
                return result;
            }
        }

        public ItemsModel GetSingle(string trackingnumber)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == trackingnumber);
                var result = _item.AssignItem(source);
                return result;
            }
        }

        public List<TbItemStatus> GetStatus()
        {
            using (var db = new EntityContext())
            {
                return db.tbItemStatuses.ToList();
            }
        }

        public bool UndoSubmit(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 1);
                item.ConfrimDate = null;
                db.SaveChanges();
                return true;
            }
        }

        public bool Submit(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 2);
                item.ConfrimDate = DateTime.Now.Date;
                db.SaveChanges();
                return true;
            }
        }

        public int Update(ItemsModel model)
        {
            using (var db = new EntityContext())
            {

                var items = _item.GetSingle(db, model.Id ?? default(int));
                if (items == null)
                {
                    return 0;
                };
                db.Entry(items).State = EntityState.Modified;
                items.ItemName = model.ItemName;
                items.ItemValue_Baht = model.ItemValue_Baht;
                items.ItemValue_Dollar = model.ItemValue_Dollar;
                items.ItemValue_Kip = model.ItemValue_Kip;
                items.ReceipverPhone = model.ReceipverPhone;
                items.ReceiverAddress = model.ReceiverAddress;
                items.ReceiverName = model.ReceiverName;
                items.Descripttion = model.Description;
                if (model.Status != null)
                {
                    items.Status = db.tbItemStatuses.FirstOrDefault(s => s.Status == model.Status);
                }
                db.SaveChanges();
                return (int)model.Id;
            }
        }


        /// <summary>
        /// For Delivery use Only
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<ItemsModel> GetProductToPickUpByCustomerId(int CustomerId, int DeliveryManId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && i.Status.Id == 2
                    && db.tbItemPickUpAllocations
                    .Where(a => a.DeliveryMan.Id == DeliveryManId).Select(a => a.Item.Id).Contains(i.Id))
                    .ToList();
                var result = _item.AssignItemsList(source);
                return result;
            }
        }

        public List<ItemsSentAllocationModel> GetProductToSend(int routeId, int timeId, DateTime? date, int userId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAllAllocation(db)
                    .Where(i => i.Route.Id == routeId
                            && i.Time.Id == timeId
                                && (i.Item.Status.Id == 5 || i.Item.Status.Id == 9 || i.Item.Status.Id == 6 || i.Item.Status.Id == 8
                                    || i.Item.Status.Id == 10)
                                    && i.DeliveryMan.Id == userId).ToList();
                if (date != null)
                {
                    source = source.Where(i => i.DateToDeliver == date).ToList();
                }
                var result = _item.AssignItemsAllocation(source);
                return result;
            }
        }

        public bool PickUp(int[] itemId, int deliveryManId)
        {
            using (var db = new EntityContext())
            {
                foreach (var id in itemId)
                {
                    var source = _item.GetSingle(db, id);
                    if (source == null)
                    {
                        return false;
                    };
                    db.Entry(source).State = EntityState.Modified;
                    source.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 3);
                    TbItemsPickupHistory pickupHistory = new TbItemsPickupHistory
                    {
                        TransactionDate = DateTime.Now.Date,
                        DeliveryMan = db.tbUsers.FirstOrDefault(u => u.Id == deliveryManId),
                        Item = db.TbItems.FirstOrDefault(i => i.Id == id)
                    };
                    db.tbItemsPickupHistories.Add(pickupHistory);
                    source.ReceiveDate = DateTime.Now.Date;
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Send(int itemId, int statusId, int deliveryManId, double? kip, double? baht, double? dollar)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                int status;
                if (item == null)
                {
                    return false;
                };
                if (statusId == 9)
                {
                    status = 8;
                }
                else
                {
                    status = 6;
                }
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == status);
                item.SentDate = DateTime.Now.Date;
                TbItemSentHistory sentHistory = new TbItemSentHistory
                {
                    DeliveryMan = db.tbUsers.FirstOrDefault(u => u.Id == deliveryManId),
                    Item = db.TbItems.FirstOrDefault(i => i.Id == itemId),
                    TransactionDate = DateTime.Now.Date,
                    IncomingBalanceInKip = kip == null ? 0 : (double)kip,
                    IncomingBalanceInBaht = baht == null ? 0 : (double)baht,
                    IncomingBalanceInDollar = dollar == null ? 0 : (double)dollar
                };
                db.tbItemSentHistories.Add(sentHistory);
                db.SaveChanges();
                return true;
            }
        }
        public bool SentButUnwantedItem(int itemId, int deliveryManId, double? kip, double? baht, double? dollar)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                int status = 10;
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == status);
                item.SentDate = DateTime.Now.Date;
                TbItemSentHistory sentHistory = new TbItemSentHistory
                {
                    DeliveryMan = db.tbUsers.FirstOrDefault(u => u.Id == deliveryManId),
                    Item = db.TbItems.FirstOrDefault(i => i.Id == itemId),
                    TransactionDate = DateTime.Now.Date,
                    IncomingBalanceInKip = kip == null ? 0 : (double)kip,
                    IncomingBalanceInBaht = baht == null ? 0 : (double)baht,
                    IncomingBalanceInDollar = dollar == null ? 0 : (double)dollar
                };
                db.tbItemSentHistories.Add(sentHistory);
                db.SaveChanges();
                return true;
            }
        }
        public List<TbRoute> GetRoute()
        {
            using (var db = new EntityContext())
            {
                return db.tbRoutes.ToList();
            }
        }

        public List<TbTime> GetTime()
        {
            using (var db = new EntityContext())
            {
                return db.tbTimes.ToList();
            }
        }



        /// <summary>
        /// For Admin use Only
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        /// 
        public double GetNotification()
        {
            using (var db = new EntityContext())
            {
                double result = _item.GetAll(db).Where(i => i.Status.Id == 2).Count();
                return result;
            }
        }

        public List<CustomerItemsModel> GetProductByCustomerId(int customerId, int statusId, DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == customerId && i.Status.Id == statusId).ToList();
                if (fromDate != null)
                {
                    source = source.Where(i => i.ReceiveDate >= fromDate).ToList();
                }
                if (toDate != null)
                {
                    source = source.Where(i => i.ReceiveDate <= toDate).ToList();
                }
                var result = _item.AssignCustomerItemsList(source);
                return result;
            }
        }

        public int CreateByAdmin(ItemsModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 3;
                    if (model.CustomerId == 0)
                    {
                        model.CustomerId = db.tbCustomers.FirstOrDefault(c => c.Phonenumber == c.Phonenumber).Id;
                    }
                    TbItems item = _item.CreateProduct(model, db, itemstatus);
                    db.Entry(item).State = EntityState.Modified;
                    item.ReceiveDate = DateTime.Now.Date;
                    db.SaveChanges();
                    dbtransact.Commit();
                    return item.Id;
                }
            }
        }

        public bool CheckTrackingNumber(string trackingNumber)
        {
            using (var db = new EntityContext())
            {
                return db.TbItems.Any(i => i.TrackingNumber == trackingNumber);
            }
        }

        public List<ItemsModel> GetProductConfirmByCustomerId(int CustomerId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && i.Status.Id == 2)
                    .ToList();
                var result = _item.AssignItemsList(source);
                return result;
            }
        }


        public List<ItemsModel> GetProductInStorePerCustomer(int CustomerId, DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && (i.Status.Id == 1 || i.Status.Id == 2)).ToList();

                if (fromDate != null)
                {
                    source = source.Where(i => i.CreatedDate >= fromDate).ToList();
                }
                if (toDate != null)
                {
                    source = source.Where(i => i.CreatedDate <= toDate).ToList();
                }
                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }

            }
        }

        public List<ItemsModel> GetProductInProcessPerCustomer(int CustomerId, DateTime? fromReceiveDate, DateTime? toReceiveDate)
        {
            using (var db = new EntityContext())
            {
                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && (i.Status.Id == 3 || i.Status.Id == 4 || i.Status.Id == 5 || i.Status.Id == 7 || i.Status.Id == 9)).ToList();

                if (fromReceiveDate != null)
                {
                    source = source.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                }
                if (toReceiveDate != null)
                {
                    source = source.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                }
                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }

            }
        }

        public List<ItemsModel> GetProductProcessedPerCustomer(int CustomerId, DateTime? fromReceiveDate, DateTime? toReceiveDate)
        {
            using (var db = new EntityContext())
            {
                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && (i.Status.Id == 6 || i.Status.Id == 8)).ToList();

                if (fromReceiveDate != null)
                {
                    source = source.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                }
                if (toReceiveDate != null)
                {
                    source = source.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                }
                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }

            }
        }

        public ItemsCountModel GetItemsCount(int CustomerId, DateTime? fromReceiveDate, DateTime? toReceiveDate)
        {
            using (var db = new EntityContext())
            {
                var totalItemReceive = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && i.ReceiveDate != null).ToList();
                var totalItem = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId
                && (i.Status.Id == 6 || i.Status.Id == 8)).ToList();
                var totalSuccess = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId
                && i.Status.Id == 6).ToList();
                var totalFailture = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId
               && i.Status.Id == 8).ToList();
                if (fromReceiveDate != null)
                {
                    totalItemReceive = totalItemReceive.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                    totalItem = totalItem.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                    totalSuccess = totalSuccess.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                    totalFailture = totalFailture.Where(i => i.ReceiveDate >= fromReceiveDate).ToList();
                }
                if (toReceiveDate != null)
                {
                    totalItemReceive = totalItemReceive.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                    totalItem = totalItem.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                    totalSuccess = totalSuccess.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                    totalFailture = totalFailture.Where(i => i.ReceiveDate <= toReceiveDate).ToList();
                }
                ItemsCountModel result = new ItemsCountModel
                {
                    totalItemReceive = totalItemReceive.Count(),
                    totalItem = totalItem.Count(),
                    totalSuccess = totalSuccess.Count(),
                    totalSendBack = totalFailture.Count()
                };
                return result;
            }
        }

        public ItemsModel ScanBarCodeItemReceive(string itemTrackingNumber)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == itemTrackingNumber);
                if (item != null)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.ReceiveDate = DateTime.Now.Date;
                    item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 4);
                    db.SaveChanges();
                    var result = _item.AssignItem(item);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
        public ItemsModel ScanBarCodeItemToSend(string itemTrackingNumber)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == itemTrackingNumber);
                if (item != null)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 5);
                    item.SendingDate = DateTime.Now.Date;
                    db.SaveChanges();
                    var result = _item.AssignItem(item);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
        public ItemsModel ScanBarCodeItemUnableToContact(string itemTrackingNumber)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == itemTrackingNumber);
                if (item != null)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 7);
                    item.SendingDate = null;
                    var allocation = _item.GetAllAllocation(db).FirstOrDefault(x => x.Item.TrackingNumber == itemTrackingNumber);
                    if (allocation != null)
                    {
                        db.tbItemSentAllocations.Remove(allocation);
                    }
                    db.SaveChanges();
                    var result = _item.AssignItem(item);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
        public ItemsModel ScanBarCodeItemToReturn(string itemTrackingNumber)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == itemTrackingNumber);
                if (item != null)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 9);
                    item.SendingDate = DateTime.Now.Date;
                    db.SaveChanges();
                    var result = _item.AssignItem(item);
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool CannotContactCustomer(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 7);
                item.SendingDate = null;
                var allocatedItem = _item.GetAllAllocation(db).FirstOrDefault(i => i.Item.Id == itemId);
                if (allocatedItem != null)
                {
                    db.tbItemSentAllocations.Remove(allocatedItem);
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateItemDescription(int itemId, string description)
        {
            using (var db = new EntityContext())
            {
                var item = _item.GetSingle(db, itemId);
                if (item != null)
                {
                    db.Entry(item).State = EntityState.Modified;
                    item.Descripttion = description;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AllocateRouteAndTime(int itemId, int routeId, int timeId, DateTime dateToDeliver)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    if (_item.GetAllAllocation(db).Any(i => i.Item.Id == itemId))
                    {
                        var item = _item.GetAllAllocation(db).FirstOrDefault(i => i.Item.Id == itemId);
                        db.tbItemSentAllocations.Remove(item);
                        db.SaveChanges();
                    };
                    var itemAllocation = new TbItemSentAllocation
                    {
                        DateToDeliver = dateToDeliver,
                        DeliveryMan = null,
                        Item = db.TbItems.FirstOrDefault(i => i.Id == itemId),
                        Route = db.tbRoutes.FirstOrDefault(r => r.Id == routeId),
                        Time = db.tbTimes.FirstOrDefault(t => t.Id == timeId)
                    };
                    var updateItem = _item.GetSingle(db, itemId);
                    db.Entry(updateItem).State = EntityState.Modified;
                    updateItem.SendingDate = dateToDeliver;
                    db.tbItemSentAllocations.Add(itemAllocation);
                    db.SaveChanges();
                    dbtransact.Commit();
                    return true;
                }

            }
        }

        public List<ItemsModel> GetProductInProcessPerCustomerAdminUser(int CustomerId)
        {
            using (var db = new EntityContext())
            {
                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && (i.Status.Id == 4 || i.Status.Id == 7)).ToList();


                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }

            }
        }

        public List<ItemsModel> GetProductTransportingPerCustomerAdminUser(int CustomerId)
        {
            using (var db = new EntityContext())
            {
                List<TbItems> source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && (i.Status.Id == 3 || i.Status.Id == 5 || i.Status.Id == 9)).ToList();


                if (source != null)
                {
                    List<ItemsModel> model = _item.AssignItemsList(source);
                    return model;
                }
                else
                {
                    return new List<ItemsModel>();
                }

            }
        }

        public ItemBarCodeModel GetBarCodeModel(int itemId)
        {
            using (var db = new EntityContext())
            {
                Zen.Barcode.Code128BarcodeDraw objBarcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;

                var item = _item.GetSingle(db, itemId);


                Image bcImage = GenCode128.Code128Rendering.MakeBarcodeImage(item.TrackingNumber, 2, true);

                ItemBarCodeModel result = new ItemBarCodeModel
                {
                    Itemname = item.ItemName,
                    Trackingnumber = item.TrackingNumber,
                    SenderName = item.Customer.Name,
                    SenderPhonenumber = item.Customer.Phonenumber,
                    ReceiverName = item.ReceiverName,
                    ReceiverPhoenumber = item.ReceipverPhone,
                    ReceiverAddress = item.ReceiverAddress,
                    Barcode = bcImage
                };
                return result;
            }
        }

        public List<ItemHistoryModel> GetItemHistory(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var customers = _customer.GetAll(db).ToList();
                List<ItemHistoryModel> model = new List<ItemHistoryModel>();
                if (customers == null)
                {
                    return new List<ItemHistoryModel>();
                };
                if (fromDate == null && toDate == null)
                {
                    foreach (var customer in customers)
                    {
                        ItemHistoryModel history = new ItemHistoryModel
                        {
                            CustomerId = customer.Id,
                            TotalItemSent = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && i.Status.Id == 6
                                  ).Count(),
                            TotalItemInProcess = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && (i.Status.Id == 3 || i.Status.Id == 4 || i.Status.Id == 5 || i.Status.Id == 7 || i.Status.Id == 9)
                                 ).Count(),
                            TotalItemReceive = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                   ).Count(),
                            TotalItemReturn = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && i.Status.Id == 8
                                  ).Count()

                        };
                        model.Add(history);
                    }
                }
                else
                {
                    foreach (var customer in customers)
                    {
                        ItemHistoryModel history = new ItemHistoryModel
                        {
                            CustomerId = customer.Id,
                            TotalItemSent = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && i.Status.Id == 6
                                    && i.ReceiveDate >= fromDate && i.ReceiveDate <= toDate).Count(),
                            TotalItemInProcess = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && (i.Status.Id == 3 || i.Status.Id == 4 || i.Status.Id == 5 || i.Status.Id == 7 || i.Status.Id == 9)
                                    && i.ReceiveDate >= fromDate && i.ReceiveDate <= toDate).Count(),
                            TotalItemReceive = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                    && i.ReceiveDate >= fromDate && i.ReceiveDate <= toDate).Count(),
                            TotalItemReturn = _item.GetAll(db)
                            .Where(i => i.Customer.Id == customer.Id
                                && i.Status.Id == 8
                                    && i.ReceiveDate >= fromDate && i.ReceiveDate <= toDate).Count()

                        };
                        model.Add(history);
                    }
                }

                return model;
            }
        }


        public List<ItemsModel> GetItemsHistoryList(DateTime? fromDate, DateTime? toDate, int condition)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).ToList();

                //1 item receive
                //2 item sent
                if (condition == 1)
                {
                    if (fromDate != null)
                    {
                        source = source.Where(x => x.ReceiveDate >= fromDate).ToList();
                    }
                    if (toDate != null)
                    {
                        source = source.Where(x => x.ReceiveDate <= toDate).ToList();
                    }


                }
                else if (condition == 2)
                {
                    source = source.Where(x => x.Status.Id == 6).ToList();
                    if (fromDate != null)
                    {
                        source = source.Where(x => x.SentDate >= fromDate).ToList();
                    }
                    if (toDate != null)
                    {
                        source = source.Where(x => x.SentDate <= toDate).ToList();
                    }
                }
                else if (condition == 3)
                {
                    source = source.Where(x => x.Status.Id == 8).ToList();
                    if (fromDate != null)
                    {
                        source = source.Where(x => x.SentDate >= fromDate).ToList();
                    }
                    if (toDate != null)
                    {
                        source = source.Where(x => x.SentDate <= toDate).ToList();
                    }
                }
                else
                {
                    source = source.ToList();
                }
                var result = _item.AssignItemsList(source);
                return result;

            }
        }

        public int GetProductId(string trackingnumber)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).FirstOrDefault(i => i.TrackingNumber == trackingnumber);
                if (source != null)
                {
                    return source.Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public List<ItemsModel> GetStatisticItemScanIn(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetAll(db).Where(i => i.ReceiveDate >= fromDate && i.ReceiveDate <= toDate).ToList();
                var result = _item.AssignItemsList(items);
                return result;
            }
        }

        public List<ItemsModel> GetStatisticItemScanSent(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetAll(db).Where(i => i.SendingDate >= fromDate
                && i.SendingDate <= toDate
                && (i.Status.Id != 8 && i.Status.Id != 9)).ToList();
                return _item.AssignItemsList(items);
            }
        }

        public ItemsCountModel GetStatisticItemScanSentCount(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetAll(db).Where(i => i.SendingDate >= fromDate
                && i.SendingDate <= toDate
                && (i.Status.Id != 8 && i.Status.Id != 9)).ToList();
                double all = items.Count();
                double success = items.Where(i => (i.Status.Id == 6 || i.Status.Id == 10) && i.SentDate != null).Count();
                ItemsCountModel result = new ItemsCountModel
                {
                    totalItem = all,
                    totalSuccess = success,
                    totalItemReceive = 0,
                    totalSendBack = 0
                };
                return result;
            }
        }

        public List<ItemsModel> GetStatisticItemSentBack(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetAll(db).Where(i => i.SendingDate >= fromDate
                && i.SendingDate <= toDate
                && (i.Status.Id == 8 || i.Status.Id == 9)).ToList();
                return _item.AssignItemsList(items);
            }
        }
        public ItemsCountModel GetStatisticItemScanReturnCount(DateTime? fromDate, DateTime? toDate)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetAll(db).Where(i => i.SendingDate >= fromDate
                && i.SendingDate <= toDate
                && (i.Status.Id == 8 || i.Status.Id == 9)).ToList();
                double all = items.Count();
                double success = items.Where(i => (i.Status.Id == 8) && i.SentDate != null).Count();
                ItemsCountModel result = new ItemsCountModel
                {
                    totalItem = all,
                    totalSuccess = success,
                    totalItemReceive = 0,
                    totalSendBack = 0
                };
                return result;
            }
        }

        public List<DeliveryHistoryModel> GetDeliveryHistory(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetSentHistory(db).Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).ToList();
                if (deliveryId != null)
                {
                    source = source.Where(x => x.DeliveryMan.Id == deliveryId).ToList();
                }
                List<DeliveryHistoryModel> model = new List<DeliveryHistoryModel>();
                if (source != null)
                {
                    model = source.Select(r => new DeliveryHistoryModel
                    {
                        Id = r.Id,
                        DeliveryDate = r.TransactionDate.ToString("dd-MM-yyyy"),
                        AllocatedSendingDate = r.Item.SendingDate == null ? "" : r.Item.SendingDate?.ToString("dd-MM-yyyy"),
                        ItemStatus = r.Item.Status.Status,
                        ItemName = r.Item.ItemName,
                        DeliveryManId = r.DeliveryMan.Id,
                        DeliveryManName = r.DeliveryMan.Username,
                        GiveMoney = r.GiveMoney,
                        IncomingBalanceInBaht = r.IncomingBalanceInBaht,
                        IncomingBalanceInDollar = r.IncomingBalanceInDollar,
                        IncomingBalanceInKip = r.IncomingBalanceInKip,
                        TrackingNumber = r.Item.TrackingNumber
                    }).ToList();
                }
                return model;
            }
        }

        public SentHistoryCountModel GetDeliveryHistoryCount(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetSentHistory(db).Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).ToList();
                if (deliveryId != null)
                {
                    source = source.Where(x => x.DeliveryMan.Id == deliveryId).ToList();
                }
                SentHistoryCountModel model = new SentHistoryCountModel
                {
                    CountItem = source.Count(),
                    KipNotYetReceive = source.Where(x => x.GiveMoney == false).Sum(x => x.IncomingBalanceInKip),
                    BahtNoYetRecieve = source.Where(x => x.GiveMoney == false).Sum(x => x.IncomingBalanceInBaht),
                    DollarNotYetReceive = source.Where(x => x.GiveMoney == false).Sum(x => x.IncomingBalanceInDollar),
                    KipReceived = source.Where(x => x.GiveMoney == true).Sum(x => x.IncomingBalanceInKip),
                    BahtReceived = source.Where(x => x.GiveMoney == true).Sum(x => x.IncomingBalanceInBaht),
                    DollarReceived = source.Where(x => x.GiveMoney == false).Sum(x => x.IncomingBalanceInDollar),
                    KipTotal = source.Sum(x => x.IncomingBalanceInKip),
                    BahtTotal = source.Sum(x => x.IncomingBalanceInBaht),
                    DollarTotal = source.Sum(x => x.IncomingBalanceInDollar)
                };
                return model;
            }
        }

        public List<PickUpHistoryModel> GetPickUpHistory(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetPickUpHistory(db).Where(x => x.TransactionDate >= fromDate && x.TransactionDate <= toDate).ToList();
                if (deliveryId != null)
                {
                    source = source.Where(x => x.DeliveryMan.Id == deliveryId).ToList();
                }
                List<PickUpHistoryModel> model = new List<PickUpHistoryModel>();
                if (source != null)
                {
                    model = source.Select(r => new PickUpHistoryModel
                    {
                        TrackingNumber = r.Item.TrackingNumber,
                        ItemStatus = r.Item.Status.Status,
                        AssignDate = r.TransactionDate == null ? "" : r.TransactionDate.ToString("dd-MM-yyyy"),
                        DeliveryManId = r.DeliveryMan.Id,
                        DeliveryManName = r.DeliveryMan.Username,
                        Id = r.Id,
                        ItemName = r.Item.ItemName
                    }).ToList();
                }
                return model;
            }
        }

        public bool ReceiveMoneyFromDeliveryMan(int historyId)
        {
            using (var db = new EntityContext())
            {
                var itemHistory = _item.GetSentHistory(db).FirstOrDefault(s => s.Id == historyId);
                if (itemHistory == null)
                {
                    return false;
                };
                db.Entry(itemHistory).State = EntityState.Modified;
                itemHistory.GiveMoney = true;
                db.SaveChanges();
                return true;
            }
        }
        public bool UndoReceiveMoneyFromDeliveryMan(int historyId)
        {
            using (var db = new EntityContext())
            {
                var itemHistory = _item.GetSentHistory(db).FirstOrDefault(s => s.Id == historyId);
                if (itemHistory == null)
                {
                    return false;
                };
                db.Entry(itemHistory).State = EntityState.Modified;
                itemHistory.GiveMoney = false;
                db.SaveChanges();
                return true;
            }
        }
    }
}
