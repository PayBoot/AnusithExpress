using System;

namespace AnousithExpress.DataEntry.Models
{
    public class TbConsolidateList
    {
        public int Id { get; set; }
        public string ConsolidateNumber { get; set; }
        public TbCustomer Customer { get; set; }
        public DateTime ConsolidatedDate { get; set; }
        public double AmountOfItem { get; set; }

        public double Fee { get; set; }
        public double IncomingBalanceInKip { get; set; }
        public double IncomingBalanceInBaht { get; set; }
        public double IncomingBalanceInDollar { get; set; }
        public double BalanceToTransfer { get; set; }

        public bool isCustomerConfirmed { get; set; }
        public bool isBalanceTransfer { get; set; }
        public bool needConfirm { get; set; }

    }
}
