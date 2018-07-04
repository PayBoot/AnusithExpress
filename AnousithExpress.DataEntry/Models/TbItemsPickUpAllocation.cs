using System;

namespace AnousithExpress.DataEntry.Models
{
    public class TbItemsPickUpAllocation
    {
        public int Id { get; set; }
        public TbItems Item { get; set; }
        public TbUser DeliveryMan { get; set; }
        public DateTime DateToPickUp { get; set; }
    }
}
