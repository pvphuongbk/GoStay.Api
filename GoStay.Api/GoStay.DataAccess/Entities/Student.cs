using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Nationality { get; set; }
        public string? MobileNo { get; set; }
        public string? ResidenceNo { get; set; }
        public string? Address { get; set; }
        public string? Password { get; set; }
        public int? IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IsDeleted { get; set; }
    }
}
