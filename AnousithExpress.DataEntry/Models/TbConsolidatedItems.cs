namespace AnousithExpress.DataEntry.Models
{
    public class TbConsolidatedItems
    {
        public int Id { get; set; }
        public TbConsolidateList Consolidator { get; set; }
        public TbItems Items { get; set; }
    }
}
