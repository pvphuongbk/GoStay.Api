using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Quan
    {
        public Quan()
        {
            Hotels = new HashSet<Hotel>();
            Phuongs = new HashSet<Phuong>();
        }

        public int Id { get; set; }
        public int? IdTinhThanh { get; set; }
        public string? Tenquan { get; set; }
        public string? Diengiai { get; set; }
        public int? Stt { get; set; }
        public int? Deleted { get; set; }
        public int? Numrecord { get; set; }
        public string? SearchKey { get; set; }

        public virtual TinhThanh? IdTinhThanhNavigation { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Phuong> Phuongs { get; set; }
    }
}
