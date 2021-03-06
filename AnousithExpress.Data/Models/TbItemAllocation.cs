﻿using System;

namespace AnousithExpress.Data.Models
{
    public class TbItemAllocation
    {
        public int Id { get; set; }
        public TbItems Item { get; set; }
        public TbRoute Route { get; set; }
        public TbTime Time { get; set; }
        public DateTime DateToDeliver { get; set; }
    }
}
