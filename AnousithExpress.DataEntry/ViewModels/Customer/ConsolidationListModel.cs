namespace AnousithExpress.DataEntry.ViewModels.Customer
{
    public class ConsolidationListModel
    {
        public int Id { get; set; }
        public string ConsolidateNumber { get; set; }
        public int CustomerID { get; set; }
        public string CustomerIdForShow
        {
            get
            {
                if (CustomerID == 0)
                {
                    return "";
                }
                else
                {
                    return CustomerID.ToString().PadLeft(4, '0');
                }
            }
        }
        public string CustomerName { get; set; }
        public string CustomerPhonenumber { get; set; }
        public string ConsolidatedDate { get; set; }
        public double AmountOfItem { get; set; }
        public double Fee { get; set; }
    }
}
