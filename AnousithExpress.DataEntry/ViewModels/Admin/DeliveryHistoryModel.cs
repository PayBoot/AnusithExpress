namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class DeliveryHistoryModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string TrackingNumber { get; set; }
        public string ItemStatus { get; set; }
        public string AllocatedSendingDate { get; set; }
        public string DeliveryDate { get; set; }
        public int DeliveryManId { get; set; }
        public string DeliveryManName { get; set; }
        public double IncomingBalanceInKip { get; set; }
        public double IncomingBalanceInBaht { get; set; }
        public double IncomingBalanceInDollar { get; set; }
        public bool GiveMoney { get; set; }
    }
}
