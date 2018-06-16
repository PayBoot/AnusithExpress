using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;

namespace AnousithExpress.Data.Interfaces
{
    public interface IItems
    {
        List<ItemSingleModel> GetAll();
        ItemSingleModel GetSingle(int id);
        bool Create(ItemSingleModel model);
        bool Update(ItemSingleModel model);
        bool Delete(int id);
        bool ConfirmItem(int itemId);
        bool UnConfirmItem(int itemId);

        List<ItemSingleModel> GetForCustomer(int CustId);
        double GetItemsAmoutPerCustomer(int CustId);

        List<ItemSingleModel> GetConfirmItems(int CustId);
        List<ItemSingleModel> GetSentItems(int CustId);
        List<ItemSingleModel> GetToSendItems(int routeId, int timeId);

        List<ItemSingleModel> GetSentItemsToConsolidate(int CustId);

        bool ReceiveItem(int[] itemId);
        bool SendItem(int itemId);
        bool AddDescription(ItemSingleModel model);
        ItemSingleModel CreateItemForDelivery(ItemSingleModel model);

        ItemSingleModel CreateItemById(ItemSingleModel model);
        List<TbItemStatus> GetItemStatus();
        List<TbRoute> GetRoute();
        List<TbTime> GetTime();


    }
}
