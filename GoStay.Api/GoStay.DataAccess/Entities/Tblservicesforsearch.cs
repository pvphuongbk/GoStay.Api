using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tblservicesforsearch
    {
        public int Id { get; set; }
        public string? Services { get; set; }
        public string? ServicesVn { get; set; }
        public int? Intstyle { get; set; }
        public string? Css { get; set; }
    }
}
