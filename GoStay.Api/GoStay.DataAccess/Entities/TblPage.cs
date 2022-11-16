using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class TblPage
    {
        public int Id { get; set; }
        public int? Idtab { get; set; }
        public string? PageName { get; set; }
        public int? Stt { get; set; }
        public int? Quyen { get; set; }
    }
}
