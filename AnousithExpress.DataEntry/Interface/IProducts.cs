using AnousithExpress.DataEntry.Models;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IProducts
    {
        /// <summary>
        /// Customer
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="CreateDate"></param>
        /// <param name="SubmitDate"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        List<ItemsModel> GetProductByCustomerId(int CustomerId,
            DateTime? CreateDate,
            DateTime? SubmitDate,
            string Status);
        ItemsModel GetSingle(int itemId);
        ItemsModel GetSingle(string trackingnumber);
        int Create(ItemsModel model);
        int Update(ItemsModel model);
        bool Submit(int itemId);
        bool UndoSubmit(int itemId);
        bool Delete(int itemId);
        List<TbItemStatus> GetStatus();
        bool CheckTrackingNumber(string trackingNumber);

        List<ItemsModel> GetProductInStorePerCustomer(int CustomerId,
            DateTime? fromCreateDate,
            DateTime? toCreateDate);

        List<ItemsModel> GetProductInProcessPerCustomer(int CustomerId,
            DateTime? fromReceiveDate,
            DateTime? toReceiveDate);
        List<ItemsModel> GetProductProcessedPerCustomer(int CustomerId,
           DateTime? fromReceiveDate,
           DateTime? toReceiveDate);
        ItemsCountModel GetItemsCount(int CustomerId,
           DateTime? fromReceiveDate,
           DateTime? toReceiveDate);
        /// Delivery
        /// 
        /// /////////////////////////////////
        /// 
        List<ItemsModel> GetProductToPickUpByCustomerId(int CustomerId);

        List<ItemsAllocationModel> GetProductToSend(int routeId, int timeId, DateTime? date, int userId);

        bool PickUp(int[] itemId);

        bool Send(int itemId, int statusId);

        List<TbRoute> GetRoute();
        List<TbTime> GetTime();


        /// Admin
        /// //////////////
        /// ///////////////////
        /// 
        ItemsModel ScanBarCodeItemReceive(string itemTrackingNumber);
        ItemsModel ScanBarCodeItemToSend(string itemTrackingNumber);
        ItemsModel ScanBarCodeItemUnableToContact(string itemTrackingNumber);
        ItemsModel ScanBarCodeItemToReturn(string itemTrackingNumber);

        Double GetNotification();
        List<CustomerItemsModel> GetProductByCustomerId(int customerId, int statusId, DateTime? fromDate, DateTime? toDate);
        int CreateByAdmin(ItemsModel model);
        List<ItemsModel> GetProductInProcessPerCustomerAdminUser(int CustomerId);
        List<ItemsModel> GetProductTransportingPerCustomerAdminUser(int CustomerId);
        bool CannotContactCustomer(int itemId);
        bool UpdateItemDescription(int itemId, string description);
        bool AllocateRouteAndTime(int itemId, int routeId, int timeId, DateTime dateToDeliver);

        ItemBarCodeModel GetBarCodeModel(int itemId);

    }
}
