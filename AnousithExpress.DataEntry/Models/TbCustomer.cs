﻿namespace AnousithExpress.DataEntry.Models
{
    public class TbCustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phonenumber { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string BCEL_Kip { get; set; }
        public string BCEL_Baht { get; set; }
        public string BCEL_Dollar { get; set; }


        public TbStatus Status { get; set; }
        public bool isDeleted { get; set; }

    }
}
