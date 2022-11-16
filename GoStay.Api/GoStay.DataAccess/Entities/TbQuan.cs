using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TbQuan
    {
        public int IdQ { get; set; }
        public int? IdTt { get; set; }
        public string? Tenquan { get; set; }
        public string? Diengiai { get; set; }
        public int? Stt { get; set; }
    }
}
