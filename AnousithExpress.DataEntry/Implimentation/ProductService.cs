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
                item.ConfrimDate = DateTime.Now;
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
        public List<ItemsModel> GetProductToPickUpByCustomerId(int CustomerId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAll(db).Where(i => i.Customer.Id == CustomerId && i.Status.Id == 2).ToList();
                var result = _item.AssignItemsList(source);
                return result;
            }
        }

        public List<ItemsAllocationModel> GetProductToSend(int routeId, int timeId, DateTime? date, int userId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAllAllocation(db)
                    .Where(i => i.Route.Id == routeId
                            && i.Time.Id == timeId
                                && (i.Item.Status.Id == 5 || i.Item.Status.Id == 9 || i.Item.Status.Id == 6 || i.Item.Status.Id == 8)
                                    && i.DeliveryMan.Id == userId).ToList();
                if (date != null)
                {
                    source = source.Where(i => i.DateToDeliver == date).ToList();
                }
                var result = _item.AssignItemsAllocation(source);
                return result;
            }
        }

        public bool PickUp(int[] itemId)
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
                    source.ReceiveDate = DateTime.Now;
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Send(int itemId, int statusId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetSingle(db, itemId);
                int status;
                if (source == null)
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
                db.Entry(source).State = EntityState.Modified;
                source.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == status);
                source.SentDate = DateTime.Now;
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
                    item.ReceiveDate = DateTime.Now;
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
                        db.tbItemAllocations.Remove(allocation);
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
                    db.tbItemAllocations.Remove(allocatedItem);
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
                        db.tbItemAllocations.Remove(item);
                        db.SaveChanges();
                    };
                    var itemAllocation = new TbItemAllocation
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
                    db.tbItemAllocations.Add(itemAllocation);
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
    }
}
