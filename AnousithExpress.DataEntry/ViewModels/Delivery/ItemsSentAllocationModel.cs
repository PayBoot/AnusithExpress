namespace AnousithExpress.DataEntry.ViewModels.Delivery
{
    public class ItemsSentAllocationModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string TrackingNumber { get; set; }

        public string ItemName { get; set; }


        public double? ItemValue_Kip { get; set; }


        public double? ItemValue_Baht { get; set; }


        public double? ItemValue_Dollar { get; set; }

        public string Status { get; set; }

        public int CustomerId { get; set; }

        public string CustomerPhonenumber { get; set; }

        public string CustomerName { get; set; }

        public string Description { get; set; }

        public string ReceiverName { get; set; }

        public string ReceipverPhone { get; set; }

        public string ReceiverAddress { get; set; }

        public string CreatedDate { get; set; }

        public string ConfrimDate { get; set; }

        public string ReceiveDate { get; set; }

        public string SendingDate { get; set; }

        public string SentDate { get; set; }

        public int StatusId { get; set; }
        public string Route { get; set; }
        public string Time { get; set; }
        public string DateToDeliver { get; set; }
        public string UserName { get; set; }
    }
}
