using GoStay.Data.Base;
using GoStay.DataDto.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Services.Comments;

public interface ICommentService
{
    public ResponseBase UpsertCommentNews(CommentNewsUpsertRequestModel request);
    public ResponseBase UpsertCommentVideo(CommentVideoUpsertRequestModel request);
    public ResponseBase GetCommentNews(int userId, int newsId, int pageIndex, int pageSize, List<CommentChildRequestModel>? listChildRequest);
    public ResponseBase GetCommentVideo(int userId, int videoId, int pageIndex, int pageSize, List<CommentChildRequestModel>? listChildRequest);
    public ResponseBase GetCommentReplyNews(int parentCommentId, int pageIndex, int pageSize);
    public ResponseBase GetCommentReplyVideo(int parentCommentId, int pageIndex, int pageSize);


}

