namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class ConsolidationListModel
    {
        public int? Id { get; set; }
        public string ConsolidateNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string ConsolidatedDate { get; set; }
        public double AmountOfItem { get; set; }

        public double Fee { get; set; }
        public double IncomingBalanceInKip { get; set; }
        public double IncomingBalanceInBaht { get; set; }
        public double IncomingBalanceInDollar { get; set; }
        public double BalanceToTransferInKip { get; set; }

        public bool isCustomerConfirmed { get; set; }
        public bool isBalanceTransfer { get; set; }
        public bool needConfirm { get; set; }

    }
}
