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
        bool CreateConsolidation(int cusomterId, double amount, double fee, int[] itemId);
        List<ItemsModel> GetUnConlidateItems(int customerId);

        double GetPrice(double condition);

        bool deleteConsolidation(int consolidationId);

        List<ConsolidationListModel> GetAllConsolidationByDate(DateTime? fromDate, DateTime? toDate);
    }
}
