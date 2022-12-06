using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class NearbyHotel
    {
        public int Id { get; set; }
        public int? IdHotel { get; set; }
        public string? Title { get; set; }
        public double? Far { get; set; }
        public int? Style { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? IdPlace { get; set; }

        public virtual Hotel? IdHotelNavigation { get; set; }
    }
}
