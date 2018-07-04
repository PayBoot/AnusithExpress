using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Collections.Generic;

namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class ConsolidationModel
    {
        public ConsolidationListModel ConsolidateDetail { get; set; }
        public List<ItemsModel> ConsolidationItems { get; set; }
    }
}
