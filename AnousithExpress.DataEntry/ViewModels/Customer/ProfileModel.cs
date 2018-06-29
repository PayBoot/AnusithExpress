using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.DataEntry.ViewModels.Customer
{
    public class ProfileModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "ກະລຸນາຕື່ມຊື່ ແລະ ນາມສະກູນ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ"), MinLength(8, ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ 8 ໂຕເລກ"), MaxLength(8, ErrorMessage = "ກະລຸນາຕື່ມເບີໂທ 8 ໂຕເລກ")]
        public string Phonenumber { get; set; }
        [Required(ErrorMessage = "ກະລຸນາໄສ່ລະຫັດຢ່າງຫນ້ອຍ 6 ຕົວເລກ"), MinLength(6, ErrorMessage = "ກະລຸນາໄສ່ລະຫັດຢ່າງຫນ້ອຍ 6 ຕົວເລກ")]
        public string Password { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "ກະລຸນາຕື່ມທີ່ຢູ່")]
        public string Address { get; set; }
        public string BCEL_Kip { get; set; }
        public string BCEL_Baht { get; set; }
        public string BCEL_Dollar { get; set; }
    }
}
