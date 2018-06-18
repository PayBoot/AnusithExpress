using AnousithExpress.Data.SingleViewModels;
using System;
using System.Collections.Generic;

namespace AnousithExpress.Data.Interfaces
{
    public interface IConsolidation
    {
        List<ConsolidateSingleModel> GetAll();
        List<ConsolidateSingleModel> GetSingle(int CustomerId, DateTime? dateFrom, DateTime? dateTo);
        ConsolidatedItemsModel GetItems(int consolidatedId);
        bool Create(ConsolidateSingleModel model);
        bool Update(ConsolidateSingleModel model);
        bool Delete(int id);
    }
}
