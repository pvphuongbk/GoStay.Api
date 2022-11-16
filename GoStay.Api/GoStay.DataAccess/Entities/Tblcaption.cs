using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tblcaption
    {
        public int Id { get; set; }
        public int? Idpage { get; set; }
        public int? Idcap { get; set; }
        public string? Lang { get; set; }
        public string? Cap { get; set; }
    }
}
