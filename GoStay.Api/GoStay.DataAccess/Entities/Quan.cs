using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Quan : BaseEntity
    {
        public Quan()
        {
            Hotels = new HashSet<Hotel>();
            Phuongs = new HashSet<Phuong>();
            TourDistrictTos = new HashSet<TourDistrictTo>();
            Tours = new HashSet<Tour>();
        }

        public int? IdTinhThanh { get; set; }
        public string? Tenquan { get; set; }
        public string? Diengiai { get; set; }
        public int? Stt { get; set; }
        public int? Numrecord { get; set; }
        public string? SearchKey { get; set; }
        public string? SanitizedName { get; set; }

        public virtual TinhThanh? IdTinhThanhNavigation { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Phuong> Phuongs { get; set; }
        public virtual ICollection<TourDistrictTo> TourDistrictTos { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
