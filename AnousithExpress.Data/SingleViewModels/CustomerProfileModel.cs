using System.Collections.Generic;

namespace AnousithExpress.Data.SingleViewModels
{
    public class CustomerProfileModel
    {

        public CustomerSingleModel customerModel { get; set; }
        public List<ItemSingleModel> itemsModel { get; set; }
    }
}
