using AnousithExpress.DataEntry.ViewModels.Admin;
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

        ConsolidateFactorModel GetConsolidationFactor(int[] itemId, int customerId);

        bool deleteConsolidation(int consolidationId);

        List<ConsolidationListModel> GetAllConsolidationByDate(DateTime? fromDate, DateTime? toDate);


        bool AddNeedConfirm(int consolidationId);
        bool UndoNeedConfirm(int consolidationId);

        bool CustomerConfirm(int consolidationId);
        bool UndoCustomerConfirm(int consolidationId);

        bool ConfirmTransfer(int consolidationId);
        bool UndoConfirmTransfer(int consolidationId);

        List<ConsolidationListModel> GetConsolidationNotification();
        double CountConfirmConsolidation();
    }
}
