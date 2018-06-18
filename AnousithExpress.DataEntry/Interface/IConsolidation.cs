using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.Interface
{
    public interface IConsolidation
    {
        //customer
        List<ConsolidationListModel> GetConsolidationListByCustomerId(int customerId, DateTime? searchFrom, DateTime? searchTo);
        ConsolidationModel GetConsolidationDetailByConsolidationId(int consolidationId);

    }
}
