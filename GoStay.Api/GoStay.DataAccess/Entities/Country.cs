using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Country
    {
        public Country()
        {
            TinhThanhs = new HashSet<TinhThanh>();
        }

        public int Id { get; set; }
        public string? Country1 { get; set; }
        public string? NameKey { get; set; }
        public string? Countrycode { get; set; }
        public string? SearchKey { get; set; }

        public virtual ICollection<TinhThanh> TinhThanhs { get; set; }
    }
}
