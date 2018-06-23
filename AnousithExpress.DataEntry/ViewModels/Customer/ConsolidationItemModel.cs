namespace AnousithExpress.DataEntry.ViewModels.Customer
{
    public class ConsolidationItemModel
    {
        public int Id { get; set; }

        public int ConsolidationId { get; set; }

        public string ConsolidationNumber { get; set; }

        public int ItemsId { get; set; }

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

        public string ReceiverPhone { get; set; }

        public string ReceiverAddress { get; set; }

        public bool isDeleted { get; set; }

        public string CreatedDate { get; set; }

        public string ConfrimDate { get; set; }

        public string ReceiveDate { get; set; }

        public string SendingDate { get; set; }

        public string SentDate { get; set; }
    }
}
