using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Status
    {
        public int StatusId { get; set; }
        public string? Title { get; set; }
        public string? Guid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IsDeleted { get; set; }
    }
}
