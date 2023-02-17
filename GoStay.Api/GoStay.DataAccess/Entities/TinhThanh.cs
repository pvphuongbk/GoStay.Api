using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TinhThanh : BaseEntity
    {
        public TinhThanh()
        {
            Hotels = new HashSet<Hotel>();
            Quans = new HashSet<Quan>();
        }

        public string? TenTt { get; set; }
        public string? Diengiai { get; set; }
        public int? Stt { get; set; }
        public int? IdCountry { get; set; }

        public string? Locality { get; set; }
        public string? Tentt2 { get; set; }
        public int? Numrecord { get; set; }
        public string? SanitizedName { get; set; }
        public string? SearchKey { get; set; }
        public string? ECode { get; set; }

        public virtual Country? IdCountryNavigation { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Quan> Quans { get; set; }
    }
}
