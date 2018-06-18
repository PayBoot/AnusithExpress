using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnousithExpress.DataEntry.Models
{
    public class TbTime
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Time { get; set; }
    }
}