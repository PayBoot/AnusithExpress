﻿using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace AnousithExpress.DataEntry.Utility
{
    public class ItemUtility
    {
        private CustomerUtility _customer = new CustomerUtility();
        public TbItems CreateProduct(ItemsModel model, EntityContext db, int itemstatus)
        {
            TbItems item = new TbItems
            {
                ItemName = model.ItemName,
                ItemValue_Baht = model.ItemValue_Baht,
                ItemValue_Dollar = model.ItemValue_Dollar,
                ItemValue_Kip = model.ItemValue_Kip,
                isDeleted = false,
                Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == itemstatus),
                Customer = _customer.GetSingle(db, model.CustomerId),
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
            return item;
        }
        public TbItems CreateProductByAdmin(CustomerItemsModel model, EntityContext db, int itemstatus)
        {
            TbItems item = new TbItems
            {
                ItemName = model.ItemName,
                ItemValue_Baht = model.ItemValue_Baht,
                ItemValue_Dollar = model.ItemValue_Dollar,
                ItemValue_Kip = model.ItemValue_Kip,
                isDeleted = false,
                Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == itemstatus),
                Customer = _customer.GetSingle(db, model.CustomerId),
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
            return item;
        }
        public TbItems GetSingle(EntityContext db, int itemId)
        {
            return GetAll(db)
                .FirstOrDefault(i => i.Id == itemId && i.isDeleted == false);
        }
        public IQueryable<TbItems> GetAll(EntityContext db)
        {
            return db.TbItems
                .Include(i => i.Status)
                .Include(i => i.Customer)
                .Where(i => i.isDeleted == false)
                .OrderByDescending(i => i.Id);
        }

        public IQueryable<TbItemSentAllocation> GetAllAllocation(EntityContext db)
        {
            return db.tbItemSentAllocations
                .Include(i => i.Item)
                .Include(i => i.Item.Customer)
                .Include(i => i.Route)
                .Include(i => i.Time)
                .Include(i => i.DeliveryMan)
                .Include(i => i.Item.Status)
                .Where(i => i.Item.isDeleted == false)
                .OrderByDescending(i => i.Id);
        }
        public IQueryable<TbItemsPickUpAllocation> GetAllPickUpAllocation(EntityContext db)
        {
            return db.tbItemPickUpAllocations
                .Include(i => i.Item)
                .Include(i => i.Item.Customer)
                .Include(i => i.DeliveryMan)
                .Include(i => i.Item.Status)
                .Where(i => i.Item.isDeleted == false)
                .OrderByDescending(i => i.Id);
        }
        public List<ItemsModel> AssignItemsList(List<TbItems> items)
        {
            if (items != null)
            {
                return items.Select(r => new ItemsModel
                {
                    Id = r.Id,
                    TrackingNumber = r.TrackingNumber,
                    CustomerId = r.Customer.Id,
                    ItemName = r.ItemName,
                    Status = r.Status.Status,
                    ItemValue_Baht = r.ItemValue_Baht,
                    ItemValue_Dollar = r.ItemValue_Dollar,
                    ItemValue_Kip = r.ItemValue_Kip,
                    CustomerPhonenumber = r.Customer.Phonenumber,
                    CustomerName = r.Customer.Name,
                    Description = r.Descripttion,
                    ReceiverName = r.ReceiverName,
                    ReceipverPhone = r.ReceipverPhone,
                    ReceiverAddress = r.ReceiverAddress,
                    ReceiveDate = r.ReceiveDate == null ? "" : r.ReceiveDate?.ToString("dd-MM-yyyy"),
                    ConfrimDate = r.ConfrimDate == null ? "" : r.ConfrimDate?.ToString("dd-MM-yyyy"),
                    CreatedDate = r.CreatedDate == null ? "" : r.CreatedDate?.ToString("dd-MM-yyyy"),
                    SendingDate = r.SendingDate == null ? "" : r.SendingDate?.ToString("dd-MM-yyyy"),
                    SentDate = r.SentDate == null ? "" : r.SentDate?.ToString("dd-MM-yyyy"),
                    isDeleted = r.isDeleted
                }).ToList();
            }
            else
            {
                return new List<ItemsModel>();
            }

        }

        public List<CustomerItemsModel> AssignCustomerItemsList(List<TbItems> items)
        {
            if (items != null)
            {
                return items.Select(r => new CustomerItemsModel
                {
                    Id = r.Id,
                    TrackingNumber = r.TrackingNumber,
                    CustomerId = r.Customer.Id,
                    ItemName = r.ItemName,
                    Status = r.Status.Status,
                    ItemValue_Baht = r.ItemValue_Baht,
                    ItemValue_Dollar = r.ItemValue_Dollar,
                    ItemValue_Kip = r.ItemValue_Kip,
                    CustomerPhonenumber = r.Customer.Phonenumber,
                    Description = r.Descripttion,
                    ReceiverName = r.ReceiverName,
                    ReceipverPhone = r.ReceipverPhone,
                    ReceiverAddress = r.ReceiverAddress,
                    ReceiveDate = r.ReceiveDate == null ? "" : r.ReceiveDate?.ToString("dd-MM-yyyy"),
                    ConfrimDate = r.ConfrimDate == null ? "" : r.ConfrimDate?.ToString("dd-MM-yyyy"),
                    CreatedDate = r.CreatedDate == null ? "" : r.CreatedDate?.ToString("dd-MM-yyyy"),
                    SendingDate = r.SendingDate == null ? "" : r.SendingDate?.ToString("dd-MM-yyyy"),
                    SentDate = r.SentDate == null ? "" : r.SentDate?.ToString("dd-MM-YYYY"),
                    isDeleted = r.isDeleted
                }).ToList();
            }
            else
            {
                return new List<CustomerItemsModel>();
            }

        }
        public List<ItemsSentAllocationModel> AssignItemsAllocation(List<TbItemSentAllocation> items)
        {
            if (items != null)
            {
                return items.Select(r => new ItemsSentAllocationModel
                {
                    ItemId = r.Item.Id,
                    DateToDeliver = r.DateToDeliver.ToString("dd-MM-yyyy"),
                    ItemName = r.Item.ItemName,
                    TrackingNumber = r.Item.TrackingNumber,
                    CustomerId = r.Item.Customer.Id,
                    CustomerName = r.Item.Customer.Name,
                    ReceiverName = r.Item.ReceiverName,
                    ReceipverPhone = r.Item.ReceipverPhone,
                    ReceiverAddress = r.Item.ReceiverAddress,
                    Description = r.Item.Descripttion,
                    CustomerPhonenumber = r.Item.Customer.Phonenumber,
                    ItemValue_Kip = r.Item.ItemValue_Kip,
                    ItemValue_Baht = r.Item.ItemValue_Baht,
                    ItemValue_Dollar = r.Item.ItemValue_Dollar,
                    Status = r.Item.Status.Status,
                    CreatedDate = r.Item.CreatedDate == null ? "" : r.Item.CreatedDate?.ToString("dd-MM-yyyy"),
                    ConfrimDate = r.Item.ConfrimDate == null ? "" : r.Item.ConfrimDate?.ToString("dd-MM-yyyy"),
                    ReceiveDate = r.Item.ReceiveDate == null ? "" : r.Item.ReceiveDate?.ToString("dd-MM-yyyy"),
                    SendingDate = r.Item.SendingDate == null ? "" : r.Item.SendingDate?.ToString("dd-MM-yyyy"),
                    SentDate = r.Item.SentDate == null ? "" : r.Item.SentDate?.ToString("dd-MM-yyyy"),
                    Id = r.Id,
                    StatusId = r.Item.Status.Id,
                    Route = r.Route.Route,
                    Time = r.Time.Time,
                    UserName = r.DeliveryMan == null ? "" : r.DeliveryMan.Username
                }).ToList();
            }
            else
            {
                return new List<ItemsSentAllocationModel>();
            }
        }

        public List<ItemsPickUpAllocationModel> AssignItemsPickUpAllocation(List<TbItemsPickUpAllocation> items)
        {
            if (items != null)
            {
                List<ItemsPickUpAllocationModel> model = new List<ItemsPickUpAllocationModel>();
                model = items.Select(r => new ItemsPickUpAllocationModel
                {
                    Id = r.Id,
                    DateToDeliver = r.DateToPickUp.ToString("dd/MM/yyyy"),
                    CustomerId = r.Item.Customer.Id,
                    CustomerName = r.Item.Customer.Name,
                    Status = r.Item.Status.Status,
                    StatusId = r.Item.Status.Id,
                    CustomerPhonenumber = r.Item.Customer.Phonenumber,
                    ItemId = r.Item.Id,
                    ItemName = r.Item.ItemName,
                    TrackingNumber = r.Item.TrackingNumber,
                    UserName = r.DeliveryMan.Username,
                    CustomerAddress = r.Item.Customer.Address
                }).ToList();
                return model;
            }
            else
            {
                return new List<ItemsPickUpAllocationModel>();
            }
        }
        public ItemsModel AssignItem(TbItems item)
        {
            if (item != null)
            {
                return new ItemsModel
                {
                    Id = item.Id,
                    TrackingNumber = item.TrackingNumber,
                    CustomerId = item.Customer.Id,
                    ItemName = item.ItemName,
                    Status = item.Status.Status,
                    ItemValue_Baht = item.ItemValue_Baht,
                    ItemValue_Dollar = item.ItemValue_Dollar,
                    ItemValue_Kip = item.ItemValue_Kip,
                    CustomerPhonenumber = item.Customer.Phonenumber,
                    CustomerName = item.Customer.Name,
                    Description = item.Descripttion,
                    ReceiverName = item.ReceiverName,
                    ReceipverPhone = item.ReceipverPhone,
                    ReceiverAddress = item.ReceiverAddress,
                    ReceiveDate = item.ReceiveDate == null ? "" : item.ReceiveDate?.ToString("dd-MM-yyyy"),
                    ConfrimDate = item.ConfrimDate == null ? "" : item.ConfrimDate?.ToString("dd-MM-yyyy"),
                    CreatedDate = item.CreatedDate == null ? "" : item.CreatedDate?.ToString("dd-MM-yyyy"),
                    SendingDate = item.SendingDate == null ? "" : item.SendingDate?.ToString("dd-MM-yyyy"),
                    SentDate = item.SentDate == null ? "" : item.SentDate?.ToString("dd-MM-yyyy"),
                    isDeleted = item.isDeleted,

                };
            }
            else
            {
                return null;
            }

        }

        public IQueryable<TbItemSentHistory> GetSentHistory(EntityContext db)
        {
            return db.tbItemSentHistories
                    .Include(x => x.DeliveryMan)
                    .Include(x => x.Item)
                    .Include(x => x.Item.Status)
                    .Include(x => x.Item.Customer)
                    .Where(x => x.Item.isDeleted == false);
        }
        public IQueryable<TbItemsPickupHistory> GetPickUpHistory(EntityContext db)
        {
            return db.tbItemsPickupHistories
                .Include(x => x.DeliveryMan)
                    .Include(x => x.Item)
                    .Include(x => x.Item.Status)
                    .Include(x => x.Item.Customer)
                    .Where(x => x.Item.isDeleted == false);

        }
    }
}
