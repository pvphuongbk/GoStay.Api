using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class CompareTour
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string IdTours { get; set; } = null!;
        public string Session { get; set; } = null!;
        public bool Deleted { get; set; }
    }
}
