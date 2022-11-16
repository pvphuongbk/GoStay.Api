using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public int? Status { get; set; }
        public int? CourseCategoryId { get; set; }
        public string? Description { get; set; }
        public string? Guid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IsDeleted { get; set; }
    }
}
