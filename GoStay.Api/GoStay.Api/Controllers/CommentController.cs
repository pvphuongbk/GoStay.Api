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
        [HttpPost("list-comment-news")]
        public ResponseBase GetCommentNews(GetCommentNewsRequestModel request)
        {
            var items = _commentService.GetCommentNews(request.UserId, request.NewsId, request.PageIndex, request.PageSize, request.ListChildRequest);
            return items;
        }
        [HttpPost("list-comment-video")]
        public ResponseBase GetCommentVideo(GetCommentVideoRequestModel request)
        {
            var items = _commentService.GetCommentVideo(request.UserId, request.VideoId, request.PageIndex, request.PageSize, request.ListChildRequest);
            return items;
        }
        [HttpGet("list-comment-reply-news")]
        public ResponseBase GetCommentReplyNews(int parentCommentId, int pageIndex, int pageSize)
        {
            var items = _commentService.GetCommentReplyNews(parentCommentId, pageIndex, pageSize);
            return items;
        }
        [HttpGet("list-comment-reply-video")]
        public ResponseBase GetCommentReplyVideo(int parentCommentId, int pageIndex, int pageSize)
        {
            var items = _commentService.GetCommentReplyVideo(parentCommentId, pageIndex, pageSize);
            return items;
        }

        [HttpGet("list-comment-for-approval-video")]
        public async Task<ResponseBase> GetCommentVideoForApproval(string? videoTitle, bool? publish, int? categoryId, int pageIndex, int pageSize)
        {
            var items =await _commentService.GetCommentVideoForApproval(videoTitle, publish, categoryId, pageIndex, pageSize);
            return items;
        }
        [HttpGet("list-comment-for-approval-news")]
        public async Task<ResponseBase> GetCommentNewsForApproval(string? newsTitle, bool? publish, int? categoryId, int? topicId, int pageIndex, int pageSize)
        { 
            var items =await _commentService.GetCommentNewsForApproval(newsTitle, publish, categoryId, topicId, pageIndex, pageSize);
            return items;
        }
        [HttpDelete("delete-comment-video")]
        public ResponseBase DeleteCommentVideo(int id)
        {
            var items = _commentService.DeleteCommentVideo(id);
            return items;
        }
        [HttpDelete("delete-comment-news")]
        public ResponseBase DeleteCommentNews(int id)
        {
            var items = _commentService.DeleteCommentNews(id);
            return items;
        }
        [HttpGet("publish-comment-news")]
        public ResponseBase PublishCommentNews(int id)
        {
            var items = _commentService.PublishCommentNews(id);
            return items;
        }
        [HttpGet("publish-comment-video")]
        public ResponseBase PublishCommentVideo(int id)
        {
            var items = _commentService.PublishCommentVideo(id);
            return items;
        }
    }
}
