namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class ItemHistoryModel
    {
        public int CustomerId { get; set; }
        public string CustomerIdForshow
        {
            get { return CustomerId.ToString().PadLeft(4, '0'); }
        }
        public double TotalItemReceive { get; set; }
        public string TotalItemReceiveForShow
        {
            get { return TotalItemReceive.ToString(); }
        }
        public double TotalItemSent { get; set; }
        public string TotalItemSentForShow
        {
            get { return TotalItemSent.ToString(); }
        }
        public double TotalItemReturn { get; set; }
        public string TotalItemReturnForShow { get { return TotalItemReturn.ToString(); } }
        public double TotalItemInProcess { get; set; }
        public string TotalItemInProcessForShow { get { return TotalItemInProcess.ToString(); } }
    }
}
