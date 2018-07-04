using System;

namespace AnousithExpress.DataEntry.Models
{
    public class TbItemsPickupHistory
    {
        public int Id { get; set; }
        public TbUser DeliveryMan { get; set; }
        public TbItems Item { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
