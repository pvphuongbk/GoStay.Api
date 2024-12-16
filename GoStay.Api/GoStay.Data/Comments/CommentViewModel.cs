﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Comments
{
    public class CommentNewsResponseModel
    {
        public List<CommentNewsViewModel> ListComment { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }=string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public int CurentQuantity { get; set; }
    }
    public class CommentNewsViewModel
    {
        public CommentNewsModel Comment { get; set; } = new();
        public CommentReplyNewsModel ReplyComments { get; set; } = new();
    }
    public class CommentNewsModel
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        
        public int UserId { get; set; }
        public string Username { get; set; }=string.Empty;
        public string UserAvatar {  get; set; }=string.Empty;
        public string Content { get; set; } = null!;
        public int ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CommentReplyNewsModel
    {
        public int ParentCommentId { get; set; }

        public List<CommentNewsModel> ReplyComments { get; set; } = new();
        public int TotalQuantityRep { get; set; }
        public int CurentQuantityRep { get; set; }
    }
    public class CommentVideoResponseModel
    {
        public List<CommentVideoViewModel> ListComment { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int TotalQuantity { get; set; }
        public int CurentQuantity { get; set; }
    }
    public class CommentVideoViewModel
    {
        public CommentVideoModel Comment { get; set; } = new();
        public CommentReplyVideoModel ReplyComments { get; set; } = new();
    }
    public class CommentVideoModel
    {
        public int Id { get; set; }
        public int VideoId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserAvatar { get; set; } = string.Empty;
        public string Content { get; set; } = null!;
        public int ParentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CommentReplyVideoModel
    {
        public int ParentCommentId { get; set; }

        public List<CommentVideoModel> ReplyComments { get; set; } = new();
        public int TotalQuantityRep { get; set; }
        public int CurentQuantityRep { get; set; }
    }
}
