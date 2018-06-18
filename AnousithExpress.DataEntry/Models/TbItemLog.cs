namespace AnousithExpress.DataEntry.Models
{
    public class TbItemLog
    {
        public int Id { get; set; }
        public TbUser User { get; set; }
        public TbItems Item { get; set; }
        public string Description { get; set; }

    }
}
