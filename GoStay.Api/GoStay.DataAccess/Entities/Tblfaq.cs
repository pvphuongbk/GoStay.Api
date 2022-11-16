using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tblfaq
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? Date { get; set; }
        public int? Stt { get; set; }
    }
}
