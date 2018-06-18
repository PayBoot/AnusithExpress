using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnousithExpress.DataEntry.Models
{
    public class TbItems
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string ItemName { get; set; }
        public double? ItemValue_Kip { get; set; }
        public double? ItemValue_Baht { get; set; }
        public double? ItemValue_Dollar { get; set; }
        public TbItemStatus Status { get; set; }
        public TbCustomer Customer { get; set; }
        public string ReceiverName { get; set; }
        public string ReceipverPhone { get; set; }
        public string ReceiverAddress { get; set; }
        public bool isDeleted { get; set; }

        public string Descripttion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ConfrimDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReceiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SendingDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SentDate { get; set; }
    }
}
