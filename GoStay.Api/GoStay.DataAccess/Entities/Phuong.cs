using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Phuong
    {
        public Phuong()
        {
            Hotels = new HashSet<Hotel>();
        }

        public int Id { get; set; }
        public string? Tenphuong { get; set; }
        public int? IdQuan { get; set; }
        public byte? Stt { get; set; }
        public string? Tenphuong2 { get; set; }
        public int? Deleted { get; set; }

        public virtual Quan? IdQuanNavigation { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
