namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerIdShow
        {
            get { return CustomerId.ToString().PadLeft(4, '0'); }
        }
        public double NumberOfConfirmItem { get; set; }
    }
}
