using System;

namespace AnousithExpress.Data.Models
{
    public class TbConsolidateList
    {
        public int Id { get; set; }
        public string ConsolidateNumber { get; set; }
        public TbCustomer Customer { get; set; }
        public DateTime ConsolidatedDate { get; set; }
        public double AmountOfItem { get; set; }
        public double Fee { get; set; }

    }
}
