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

        List<ItemSingleModel> GetForCustomer(int CusId);
        double GetItemsAmoutPerCustomer(int CusId);

        List<ItemSingleModel> GetConfirmItems();
    }
}
