using System.Drawing;

namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class ItemBarCodeModel
    {
        public string Itemname { get; set; }
        public string Trackingnumber { get; set; }
        public string SenderName { get; set; }
        public string SenderPhonenumber { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhoenumber { get; set; }
        public string ReceiverAddress { get; set; }
        public Image Barcode { get; set; }
    }
}
