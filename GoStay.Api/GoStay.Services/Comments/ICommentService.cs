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
    public ResponseBase GetCommentNews(int userId, int newsId, int pageIndex, int pageSize);


}

