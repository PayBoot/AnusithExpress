using System;

namespace AnousithExpress.DataEntry.Models
{
    public class TbItemSentHistory
    {
        public int Id { get; set; }
        public TbUser DeliveryMan { get; set; }
        public TbItems Item { get; set; }
        public DateTime TransactionDate { get; set; }
        public double IncomingBalanceInKip { get; set; }
        public double IncomingBalanceInBaht { get; set; }
        public double IncomingBalanceInDollar { get; set; }
        public bool GiveMoney { get; set; }
    }
}
