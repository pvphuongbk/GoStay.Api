using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelFlashSale 
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public bool IsPin { get; set; }
        public bool Deleted { get; set; }

    }
}
