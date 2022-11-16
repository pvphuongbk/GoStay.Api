using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class AspModule
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Link { get; set; } = null!;
        public int IsEf { get; set; }
        public string? Logo { get; set; }
        public int Status { get; set; }
    }
}
