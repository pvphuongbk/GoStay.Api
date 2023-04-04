using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class HotelMameniti 
    {
        public int Idhotel { get; set; }
        public int Idservices { get; set; }
        public int Id { get; set; }
        public byte? Level { get; set; }

        public virtual Hotel IdhotelNavigation { get; set; } = null!;
        public virtual Service IdservicesNavigation { get; set; } = null!;
    }
}
