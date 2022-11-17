﻿using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TinhThanh
    {
        public TinhThanh()
        {
            Hotels = new HashSet<Hotel>();
            Quans = new HashSet<Quan>();
        }

        public int Id { get; set; }
        public string? TenTt { get; set; }
        public string? Diengiai { get; set; }
        public int? Stt { get; set; }
        public string? Locality { get; set; }
        public string? Tentt2 { get; set; }
        public int? Deleted { get; set; }

        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Quan> Quans { get; set; }
    }
}
