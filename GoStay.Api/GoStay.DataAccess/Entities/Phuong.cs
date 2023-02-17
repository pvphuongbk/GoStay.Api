using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Phuong : BaseEntity
    {
        public Phuong()
        {
            Hotels = new HashSet<Hotel>();
        }

        public string? Tenphuong { get; set; }
        public int? IdQuan { get; set; }
        public byte? Stt { get; set; }
        public string? Tenphuong2 { get; set; }
        public int? Numrecord { get; set; }
        public string? SearchKey { get; set; }

        public virtual Quan? IdQuanNavigation { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
