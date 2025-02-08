using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Comments
{
    public class GetCommentNewsRequestModel
    {
        public int UserId { get; set; }
        public int NewsId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<CommentChildRequestModel>? ListChildRequest { get; set; } = new();
    }
    public class GetCommentVideoRequestModel
    {
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<CommentChildRequestModel>? ListChildRequest { get; set; } = new();
    }
    public class CommentChildRequestModel
    {
        public int ParentId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
