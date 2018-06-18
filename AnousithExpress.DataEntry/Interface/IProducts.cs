using AnousithExpress.DataEntry.Models;
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
        bool Create(ItemsModel model);
        bool Update(ItemsModel model);
        bool Submit(int itemId);
        bool UndoSubmit(int itemId);
        bool Delete(int itemId);
        List<TbItemStatus> GetStatus();


        /// Delivery
        /// 
        /// /////////////////////////////////
        /// 
        List<ItemsModel> GetProductToPickUpByCustomerId(int CustomerId);

        List<ItemsAllocationModel> GetProductToSend(int routeId, int timeId, DateTime? date);

        bool PickUp(int[] itemId);

        bool Send(int itemId);

        List<TbRoute> GetRoute();
        List<TbTime> GetTime();


        /// Admin
        /// //////////////
        /// ///////////////////
        /// 
        Double GetNotification();


    }
}
