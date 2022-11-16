using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tblservicesforcontent
    {
        public int Id { get; set; }
        public int Idhotel { get; set; }
        public int Idservices { get; set; }
        public string? Content { get; set; }
        public int? Intlang { get; set; }
    }
}
