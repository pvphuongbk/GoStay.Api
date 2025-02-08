using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class CommentVideo
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public int ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual VideoNews Video { get; set; } = null!;
    }
}
