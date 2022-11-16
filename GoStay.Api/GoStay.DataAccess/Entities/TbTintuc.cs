using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TbTintuc
    {
        public int IdTin { get; set; }
        public string? Title { get; set; }
        public string? Tomtat { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; }
        public int? IdU { get; set; }
        public int? IdNt { get; set; }
        public int? Numclick { get; set; }
    }
}
