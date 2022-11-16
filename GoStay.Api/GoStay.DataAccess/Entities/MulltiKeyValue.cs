using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class MulltiKeyValue
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public double? FloatValue { get; set; }
        public string? TextValue { get; set; }
        public int? IdGroup { get; set; }
        public string? Icon { get; set; }
    }
}
