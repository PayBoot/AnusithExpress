using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.Data.SingleViewModels
{
    public class ItemSingleModel
    {
        public int? Id { get; set; }
        public string TrackingNumber { get; set; }

        [Required(ErrorMessage = "ກະລຸນາຕື່ມປະເພດສີນຄ້າ")]
        public string ItemName { get; set; }
        public double? ItemValue_Kip { get; set; }
        public double? ItemValue_Baht { get; set; }
        public double? ItemValue_Dollar { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }

        public string CustomerPhonenumber { get; set; }

        [Required(ErrorMessage = "ກະລຸນາຕື່ມຊື່ຜູ້ຮັບ")]
        public string ReceiverName { get; set; }

        [Required(ErrorMessage = "ກະລຸນາຕື່ມເບີໂທຜູ້ຮັບ")]
        public string ReceipverPhone { get; set; }

        [Required(ErrorMessage = "ກະລຸນາຕື່ມທີ່ຢູ່")]
        public string ReceiverAddress { get; set; }
        public bool isDeleted { get; set; }

        public string CreatedDate { get; set; }
        public string ConfrimDate { get; set; }
        public string ReceiveDate { get; set; }
        public string SendingDate { get; set; }
    }
}
