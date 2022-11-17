using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class KhuVuc
    {
        public int IdKv { get; set; }
        public int? IdQ { get; set; }
        public string? Tenkv { get; set; }
        public int? Stt { get; set; }
        public string? Diengiai { get; set; }
        public byte? Active { get; set; }
    }
}
