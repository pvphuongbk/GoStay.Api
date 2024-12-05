using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataDto.Comments;
using GoStay.DataDto.HotelDto;
using GoStay.Services;
using GoStay.Services.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoStay.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpPost("comment-news")]
        public ResponseBase UpsertCommentNews(CommentNewsUpsertRequestModel request)
        {
            var items = _commentService.UpsertCommentNews(request);
            return items;
        }
        [HttpPost("comment-video")]
        public ResponseBase UpsertCommentVideo(CommentVideoUpsertRequestModel request)
        {
            var items = _commentService.UpsertCommentVideo(request);
            return items;
        }
        [HttpGet("list-comment-news")]
        public ResponseBase GetCommentNews(int userId, int newsId, int pageIndex, int pageSize)
        {
            var items = _commentService.GetCommentNews(userId,newsId,pageIndex,pageSize);
            return items;
        }
    }
}
