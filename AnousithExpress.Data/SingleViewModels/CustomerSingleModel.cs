using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.Data.SingleViewModels
{
    public class CustomerSingleModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage ="ກະລຸນາຕື່ມຊື່ ແລະນາມສະກຸນ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ"), MinLength(8, ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ 8 ໂຕເລກ"), MaxLength(8, ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ 8 ໂຕເລກ")]
        public string Phonenumber { get; set; }
        [Required(ErrorMessage = "ກະລຸນາໄສ່ລະຫັດຢ່າງໜ້ອຍ 6 ໂຕເລກ"), MinLength(6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ກະລຸນາຕື່ມທີ່ຢູ່")]
        public string Address { get; set; }
        public string BCEL_Kip { get; set; }
        public string BCEL_Baht { get; set; }
        public string BCEL_Dollar { get; set; }
        public string Status { get; set; }
        public bool isDeleted { get; set; }
    }
}
