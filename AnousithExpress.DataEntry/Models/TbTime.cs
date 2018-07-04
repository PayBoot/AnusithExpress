using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.DataEntry.Models
{
    public class TbTime
    {
        public int Id { get; set; }


        [StringLength(100)]
        [Display(Name = "ເວລາສົ່ງ")]
        public string Time { get; set; }
    }
}