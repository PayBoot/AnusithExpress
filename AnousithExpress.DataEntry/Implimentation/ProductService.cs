using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.Utility;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public bool Create(ItemsModel model)
        {
            using (var db = new EntityContext())
            {
                using (var dbtransact = db.Database.BeginTransaction())
                {
                    int itemstatus = 1;
                    TbItems item = _item.CreateProduct(model, db, itemstatus);
                    dbtransact.Commit();
                    return true;
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

        public bool Update(ItemsModel model)
        {
            using (var db = new EntityContext())
            {
                var items = _item.GetSingle(db, model.Id ?? default(int));
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

        public List<ItemsAllocationModel> GetProductToSend(int routeId, int timeId, DateTime? date)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetAllAllocation(db)
                    .Where(i => i.Route.Id == routeId
                            && i.Time.Id == timeId).ToList();
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

        public bool Send(int itemId)
        {
            using (var db = new EntityContext())
            {
                var source = _item.GetSingle(db, itemId);
                if (source == null)
                {
                    return false;
                };
                db.Entry(source).State = EntityState.Modified;
                source.Status = db.tbItemStatuses.FirstOrDefault(s => s.Id == 6);
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
    }
}
