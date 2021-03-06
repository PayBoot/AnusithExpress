﻿using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using AnousithExpress.Data.UtilityClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace AnousithExpress.Data.Implementation
{
    public class ItemService : IItems
    {
        private CustomerUtility customerUtility;
        private ItemsUtility itemsUtility;
        public ItemService(CustomerUtility custUtility, ItemsUtility itemUtility)
        {
            customerUtility = custUtility;
            itemsUtility = itemUtility;

        }

        public bool Create(ItemSingleModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 1;
                    TbItems item = CreateItemMethod(model, db, dbtransact, itemstatus);
                    return true;
                }

            }
        }

        public bool Delete(int id)
        {
            using (var db = new EntityContext())
            {
                var item = itemsUtility.GetItemById(id, db);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.isDeleted = true;
                db.SaveChanges();
                return true;
            }
        }

        public List<ItemSingleModel> GetAll()
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).ToList(); //Get data from database
                var model = itemsUtility.AssingItemsProperty(items); //post data to model
                return model;
            }
        }

        public List<ItemSingleModel> GetForCustomer(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).Where(i => i.Customer.Id == CustId).ToList();
                var model = itemsUtility.AssingItemsProperty(items);
                return model;
            }
        }

        public List<ItemSingleModel> GetConfirmItems(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).Where(i => i.Status.Id == 2 && i.Customer.Id == CustId).ToList();
                var model = itemsUtility.AssingItemsProperty(items);
                return model;
            }
        }

        public double GetItemsAmoutPerCustomer(int CustId)
        {
            using (var db = new EntityContext())
            {
                double itemsNumber = itemsUtility.GetAllItem(db).Where(i => i.Customer.Id == CustId).Count();
                return itemsNumber;
            }
        }

        public bool Update(ItemSingleModel model)
        {
            using (var db = new EntityContext())
            {

                var items = itemsUtility.GetItemById(model.Id ?? default(int), db);
                if (items == null)
                {
                    return false;
                };
                db.Entry(items).State = EntityState.Modified;
                items.ItemName = model.ItemName;
                items.Customer = db.tbCustomers.FirstOrDefault(x => x.Id == model.CustomerId);
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
                return true;

            }
        }

        public ItemSingleModel GetSingle(int id)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetItemById(id, db);
                if (items == null)
                {
                    return null;
                }
                var model = new ItemSingleModel
                {
                    Id = items.Id,
                    ItemName = items.ItemName,
                    Status = items.Status.Status,
                    CustomerId = items.Customer.Id,
                    CustomerPhonenumber = items.Customer.Phonenumber,
                    Description = items.Descripttion,
                    isDeleted = items.isDeleted,
                    ItemValue_Baht = items.ItemValue_Baht,
                    ItemValue_Dollar = items.ItemValue_Dollar,
                    ItemValue_Kip = items.ItemValue_Kip,
                    ReceipverPhone = items.ReceipverPhone,
                    ReceiverAddress = items.ReceiverAddress,
                    ReceiverName = items.ReceiverName,
                    TrackingNumber = items.TrackingNumber,
                    ReceiveDate = items.ReceiveDate?.ToString("dd-MM-yyyy"),
                    SendingDate = items.SendingDate?.ToString("dd-MM-yyyy"),
                    ConfrimDate = items.ConfrimDate?.ToString("dd-MM-yyyy"),
                    CreatedDate = items.CreatedDate?.ToString("dd-MM-yyyy")
                };
                return model;
            }
        }

        public bool ReceiveItem(int[] itemId)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var id in itemId)
                        {
                            var item = itemsUtility.GetItemById(id, db);
                            if (item == null)
                            {
                                dbtransact.Rollback();
                                return false;
                            };
                            db.Entry(item).State = EntityState.Modified;
                            item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 3);
                            item.ReceiveDate = DateTime.Now;
                            db.SaveChanges();
                        }
                        dbtransact.Commit();
                        return true;
                    }
                    catch
                    {

                        dbtransact.Rollback();
                        return false;
                    }

                }

            }
        }

        public List<ItemSingleModel> GetToSendItems(int routeId, int timeId, DateTime? dateToDeliver)
        {
            using (var db = new EntityContext())
            {
                IQueryable<TbItemAllocation> item;
                if (dateToDeliver == null)
                {
                    item = itemsUtility.GetAllItemAllocation(db)
                   .Where(i => i.Route.Id == routeId && i.Time.Id == timeId);
                }
                else
                {
                    item = itemsUtility.GetAllItemAllocation(db)
                   .Where(i => i.Route.Id == routeId && i.Time.Id == timeId && i.DateToDeliver == dateToDeliver);
                }
                if (item != null)
                {
                    var model = itemsUtility.AssignItemsProperty(item.ToList());
                    return model;
                }
                else
                {
                    return new List<ItemSingleModel>();
                }
            }
        }

        public bool SendItem(int itemId)
        {
            using (var db = new EntityContext())
            {
                if (itemsUtility.GetItemById(itemId, db) != null)
                {
                    var item = itemsUtility.GetItemById(itemId, db);
                    db.Entry(item).State = EntityState.Modified;
                    item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 6);
                    item.SendingDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                return true;
            }
        }

        public List<TbRoute> GetRoute()
        {
            using (var db = new EntityContext())
            {
                var route = db.tbRoutes;
                if (route != null)
                {
                    return route.ToList();
                }
                else
                {
                    return new List<TbRoute>();
                }
            }
        }

        public List<TbTime> GetTime()
        {
            using (var db = new EntityContext())
            {
                var time = db.tbTimes;
                if (time != null)
                {
                    return time.ToList();
                }
                else
                {
                    return new List<TbTime>();
                }
            }
        }

        public ItemSingleModel CreateItemForDelivery(ItemSingleModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 3;
                    if (model.Status != null)
                    {
                        itemstatus = db.tbItemStatuses.FirstOrDefault(x => x.Status == model.Status).Id;
                    }
                    if (model.CustomerId == 0)
                    {
                        model.CustomerId = db.tbCustomers.FirstOrDefault(x => x.Phonenumber == model.CustomerPhonenumber).Id;
                    }
                    TbItems item = CreateItemMethod(model, db, dbtransact, itemstatus);
                    return GetSingle(item.Id);
                }

            }
        }

        private TbItems CreateItemMethod(ItemSingleModel model, EntityContext db, DbContextTransaction dbtransact, int itemstatus)
        {
            TbItems item = new TbItems
            {
                ItemName = model.ItemName,
                ItemValue_Baht = model.ItemValue_Baht,
                ItemValue_Dollar = model.ItemValue_Dollar,
                ItemValue_Kip = model.ItemValue_Kip,
                isDeleted = false,
                Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == itemstatus),
                Customer = customerUtility.GetCustomerById(db, model.CustomerId),
                CreatedDate = DateTime.Now,
                ReceipverPhone = model.ReceipverPhone,
                ReceiverAddress = model.ReceiverAddress,
                ReceiverName = model.ReceiverName,
                Descripttion = model.Description
            };
            db.TbItems.Add(item);
            db.SaveChanges();
            db.Entry(item).State = EntityState.Modified;
            string customerId = model.CustomerId.ToString().PadLeft(4, '0');
            string itemId = item.Id.ToString().PadLeft(7, '0');
            StringBuilder sb = new StringBuilder();
            sb.Append("U" + customerId);
            sb.Append("I" + itemId);
            item.TrackingNumber = sb.ToString();
            db.SaveChanges();
            dbtransact.Commit();
            return item;
        }

        public bool ConfirmItem(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = itemsUtility.GetItemById(itemId, db);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 2);
                db.SaveChanges();
                return true;
            }
        }

        public bool UnConfirmItem(int itemId)
        {
            using (var db = new EntityContext())
            {
                var item = itemsUtility.GetItemById(itemId, db);
                if (item == null)
                {
                    return false;
                };
                db.Entry(item).State = EntityState.Modified;
                item.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 1);
                db.SaveChanges();
                return true;
            }
        }

        public List<TbItemStatus> GetItemStatus()
        {
            using (var db = new EntityContext())
            {
                return db.tbItemStatuses.ToList();

            }
        }

        public List<ItemSingleModel> GetSentItems(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).Where(i => i.Customer.Id == CustId && i.Status.Id == 6);
                if (items != null)
                {
                    var model = itemsUtility.AssingItemsProperty(items.ToList());
                    return model;
                }
                else
                {
                    return new List<ItemSingleModel>();
                }
            }
        }

        public List<ItemSingleModel> GetSentItemsToConsolidate(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db)
                    .Where(i => i.Customer.Id == CustId
                        && i.Status.Id == 6
                        && !db.tbConsolidatedItems.Select(c => c.Items.Id).Contains(i.Id));
                if (items != null)
                {
                    var model = itemsUtility.AssingItemsProperty(items.ToList());
                    return model;
                }
                else
                {
                    return new List<ItemSingleModel>();
                }
            }
        }

        public bool AddDescription(ItemSingleModel model)
        {
            using (var db = new EntityContext())
            {
                if (model.Id > 0)
                {
                    var item = itemsUtility.GetItemById(model.Id ?? default(int), db);
                    db.Entry(item).State = EntityState.Modified;
                    item.Descripttion = model.Description;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public ItemSingleModel CreateItemById(ItemSingleModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 3;
                    TbItems item = CreateItemMethod(model, db, dbtransact, itemstatus);
                    return GetSingle(item.Id);
                }

            }
        }

        public bool AllocateItem(int itemId, int routeId, int timeId, DateTime dateToDeliver)
        {
            using (var db = new EntityContext())
            {

                if (db.tbItemAllocations.Include(x => x.Item).FirstOrDefault(x => x.Item.Id == itemId) != null)
                {
                    var item = db.tbItemAllocations.Include(x => x.Item).FirstOrDefault(x => x.Item.Id == itemId);
                    db.Entry(item).State = EntityState.Modified;
                    item.Route = db.tbRoutes.FirstOrDefault(x => x.Id == routeId);
                    item.Time = db.tbTimes.FirstOrDefault(x => x.Id == timeId);
                    item.DateToDeliver = dateToDeliver;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    var item = itemsUtility.GetItemById(itemId, db);
                    TbItemAllocation allocation = new TbItemAllocation
                    {
                        Item = item,
                        Route = db.tbRoutes.FirstOrDefault(x => x.Id == routeId),
                        Time = db.tbTimes.FirstOrDefault(x => x.Id == timeId),
                        DateToDeliver = dateToDeliver

                    };
                    db.tbItemAllocations.Add(allocation);
                    db.SaveChanges();
                    return true;
                }


            }
        }
    }
}
