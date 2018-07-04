using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.DataEntry.Models
{
    public class TbRoute
    {
        public int Id { get; set; }
        [Display(Name = "ສາຍສົ່ງ")]
        public string Route { get; set; }
    }
}