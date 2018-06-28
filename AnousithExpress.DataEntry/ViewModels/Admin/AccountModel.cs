using System.ComponentModel.DataAnnotations;

namespace AnousithExpress.DataEntry.ViewModels.Admin
{
    public class AccountModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "ກະລຸນາໃສ່ຊື່ບັນຊີ"), RegularExpression(@"^\S*$", ErrorMessage = "ກະລຸນາບໍ່ໃຊ້ບ່ອນວ່າງ")]
        public string Username { get; set; }
        [Required(ErrorMessage = "ກະລຸນາໃສ່ລະຫັດ"), MinLength(6, ErrorMessage = "ກະລຸນາໃສ່ລະຫັດຢ່າງຫນ້ອຍ 6 ໂຕເລກ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ກະລຸນາເລືອກຫນ້າທີ່")]
        public string Role { get; set; }
        public string Phonenumber { get; set; }
        [Required(ErrorMessage = "ກະລຸນາເລືອກສະຖານະ")]
        public string Status { get; set; }

    }
}
