namespace AnousithExpress.Data.SingleViewModels
{
    public class ConsolidateSingleModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ConsolidateNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhonenumber { get; set; }
        public string ConsolidateDate { get; set; }
        public double Amount { get; set; }
        public double Fee { get; set; }
        public int[] ItemId { get; set; }
    }
}
