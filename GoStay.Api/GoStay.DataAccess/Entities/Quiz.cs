using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AllowedTimeMinutes { get; set; }
        public int? Status { get; set; }
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public string? Description { get; set; }
        public int? PassingMarks { get; set; }
        public string? Guid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ModifiedBy { get; set; }
        public int? IsDeleted { get; set; }
    }
}
