using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Viewhotelservice
    {
        public int Id { get; set; }
        public int? Idhotel { get; set; }
        public string? ServicesVn { get; set; }
    }
}
