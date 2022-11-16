using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Tbluser
    {
        public int Id { get; set; }
        public string? Ten { get; set; }
        public string? Diachi { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte? IsActive { get; set; }
        public byte? UserType { get; set; }
    }
}
