namespace AnousithExpress.Data.Models
{
    public class TbUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TbRole Role { get; set; }
        public string Phonenumber { get; set; }
        public TbStatus Status { get; set; }
    }
}
