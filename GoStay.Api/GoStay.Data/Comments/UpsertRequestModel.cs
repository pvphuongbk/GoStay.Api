using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Comments
{
    public class CommentNewsUpsertRequestModel
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public int ParentId { get; set; }
    }
    public class CommentVideoUpsertRequestModel
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; } = null!;
        public int ParentId { get; set; }
    }
}
