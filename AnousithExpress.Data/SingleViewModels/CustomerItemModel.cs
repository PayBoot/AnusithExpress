namespace AnousithExpress.Data.SingleViewModels
{
    public class CustomerItemModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhonenumber { get; set; }
        public double? CountItems { get; set; }
        public double? CountItemsToPickup { get; set; }
        public double? CountItemsPickedUp { get; set; }
        public double? CountItemsInProcess { get; set; }
        public double? CountItemsToSend { get; set; }
        public double? CountItemsAlreadySend { get; set; }
        public double? CountItemsCannotContact { get; set; }
    }
}
