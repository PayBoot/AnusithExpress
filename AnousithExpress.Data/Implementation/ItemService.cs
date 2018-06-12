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
                    TbItems item = new TbItems
                    {
                        ItemName = model.ItemName,
                        ItemValue_Baht = model.ItemValue_Baht,
                        ItemValue_Dollar = model.ItemValue_Dollar,
                        ItemValue_Kip = model.ItemValue_Kip,
                        isDeleted = false,
                        Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 1),
                        Customer = customerUtility.GetCustomerById(db, model.CustomerId),
                        CreatedDate = DateTime.Now,
                        ReceipverPhone = model.ReceipverPhone,
                        ReceiverAddress = model.ReceiverAddress,
                        ReceiverName = model.ReceiverName,
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
                var items = itemsUtility.GetAllItem(db).ToList();
                var model = itemsUtility.ItemListModelProperty(items);
                return model;
            }
        }

        public List<ItemSingleModel> GetForCustomer(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).Where(i => i.Customer.Id == CustId).ToList();
                var model = itemsUtility.ItemListModelProperty(items);
                return model;
            }
        }

        public List<ItemSingleModel> GetConfirmItems(int CustId)
        {
            using (var db = new EntityContext())
            {
                var items = itemsUtility.GetAllItem(db).Where(i => i.Status.Id == 2 && i.Customer.Id == CustId).ToList();
                var model = itemsUtility.ItemListModelProperty(items);
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
                items.ItemValue_Baht = model.ItemValue_Baht;
                items.ItemValue_Dollar = model.ItemValue_Dollar;
                items.ItemValue_Kip = model.ItemValue_Kip;
                items.ReceipverPhone = model.ReceipverPhone;
                items.ReceiverAddress = model.ReceiverAddress;
                items.ReceiverName = model.ReceiverName;
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
    }
}