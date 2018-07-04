namespace AnousithExpress.DataEntry.ViewModels.Delivery
{
    public class ItemsPickUpAllocationModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string TrackingNumber { get; set; }

        public string ItemName { get; set; }

        public string Status { get; set; }

        public int CustomerId { get; set; }

        public string CustomerPhonenumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public int StatusId { get; set; }

        public string DateToDeliver { get; set; }

        public string UserName { get; set; }
    }
}
