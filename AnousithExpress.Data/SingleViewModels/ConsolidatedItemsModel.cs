using System.Collections.Generic;

namespace AnousithExpress.Data.SingleViewModels
{
    public class ConsolidatedItemsModel
    {
        public int Id { get; set; }
        public int ConsolidationId { get; set; }
        public string ConsolidationNumber { get; set; }
        public string Customername { get; set; }
        public string CustomerPhonenumber { get; set; }
        public List<ItemSingleModel> Items { get; set; }

    }
}
