namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdShow
        {
            get { return CustomerId.ToString().PadLeft(4, '0'); }
        }
        public double NumberOfConfirmItem { get; set; }
        public double NumberOfInProcessItem { get; set; }
        public double NumberOfProcessedItem { get; set; }
        public double NumberOfItemSending { get; set; }
    }
}
