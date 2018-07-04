using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.DataEntry.Models
{
    public class TbPrice
    {
        public int Id { get; set; }
        [Display(Name = "ຫົວຫນ່ວຍຄິດໄລ່")]
        public double Condition { get; set; }
        [Display(Name = "ລາຄາຕໍ່ຫົວຫນ່ວຍ")]
        public double PriceSet { get; set; }
    }
}
