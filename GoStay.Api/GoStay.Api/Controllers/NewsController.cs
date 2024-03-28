using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoStay.Data.Base;
using GoStay.Services;
using GoStay.Services.Newss;
using GoStay.DataDto.News;
using GoStay.DataAccess.Entities;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {

        private readonly INewsService _newsServices;
        private readonly IMapper _mapper;
        private readonly IMyTypedClientServices _client;
        public NewsController(INewsService newsServices, IMapper mapper, IMyTypedClientServices client)
        {
            _newsServices = newsServices;
            _mapper = mapper;
            _client = client;
        }
        [HttpGet("news-default")]
        public ResponseBase GetNewsDefault(int idUser, int idNews)
        {
            var items = _newsServices.GetNewsDefault(idUser, idNews);
            return items;
        }
        [HttpPost("listv2")]
        public ResponseBase GetListNews2(GetListNewsParam param)
        {
            var items = _newsServices.GetListNews2(param);
            return items;
        }
        [HttpPut("submit-news")]
        public ResponseBase SubmitNews(NewsDataDto newsDto)
        {
            var item = _newsServices.SubmitNews(newsDto);
            return item;
        }
        [HttpPost("list")]
        public ResponseBase GetListNews(GetListNewsParam param)
        {
            var items = _newsServices.GetListNews(param);
            return items;
        }
        [HttpGet("list-homepage")]
        public ResponseBase GetListNewsHomePage()
        {
            var items = _newsServices.GetListNewsHomePage();
            return items;
        }
        [HttpGet("news")]
        public ResponseBase GetNews(int Id)
        {
            var items = _newsServices.GetNews(Id);
            return items;
        }
        [HttpGet("video-news")]
        public ResponseBase GetVideoNews(int Id)
        {
            var items = _newsServices.GetVideoNews(Id);
            return items;
        }

        [HttpGet("news-topic-total")]
        public ResponseBase GetNewsTopicTotal(int IdDomain)
        {
            var items = _newsServices.GetNewsTopicTotal(IdDomain);
            return items;
        }
        [HttpGet("news-category-total")]
        public ResponseBase GetNewsCategoryTotal(int IdDomain)
        {
            var items = _newsServices.GetNewsCategoryTotal(IdDomain);
            return items;
        }
        [HttpGet("list-category-by-parentid")]
        public ResponseBase GetListCategoryByParentId(int IdDomain, int ParentId)
        {
            var items = _newsServices.GetListCategoryByParentId(IdDomain, ParentId);
            return items;
        }

        [HttpPost("list-video-news")]
        public ResponseBase GetListVideoNews(GetListVideoNewsParam filter)
        {
            var items = _newsServices.GetListVideoNews(filter);
            return items;
        }


        [HttpGet("top-news")] 
        public ResponseBase GetListTopNewsByCategory(int? IdCategory, int? IdTopic)
        {
            var items = _newsServices.GetListTopNewsByCategory(IdCategory, IdTopic);
            return items;
        }

        [HttpGet("data-support")] 
        public ResponseBase GetDataSupportNews()
        {
            var items = _newsServices.GetDataSupportNews();
            return items;
        }

        [HttpPost("add-news")]
        public ResponseBase AddNews(NewsDto newsDto)
        {
            var item = _newsServices.AddNews(newsDto);
            return item;
        }

        [HttpPost("add-video-news")]
        public ResponseBase AddVideoNews(VideoModel videoNews)
        {
            var item = _newsServices.AddVideoNews(videoNews);
            return item;
        }

        [HttpPut("edit-news")]
        public ResponseBase EditNews(NewsDto newsDto)
        {
            var item = _newsServices.EditNews(newsDto);
            return item;
        }

        [HttpPut("edit-content-news")]
        public ResponseBase EditContentNews(EditNewsContentParam param)
        {
            var item = _newsServices.EditContentNews(param.Content, param.NewsId);
            return item;
        }

        [HttpPut("edit-picturetitle-news")]
        public ResponseBase EditPictureTitleNews(EditNewsPictureTitleParam param)
        {
            var item = _newsServices.EditPictureTitleNews(param.Url,param.NewsId);
            return item;
        }
        [HttpPut("delete-news")]
        public ResponseBase DeleteNews([FromBody]int Id)
        {
            var item = _newsServices.DeleteNews(Id);
            return item;
        }
        [HttpPut("click")]
        public ResponseBase EditClickNews([FromBody] int Id)
        {
            var item = _newsServices.EditClickNews(Id);
            return item;
        }
        [HttpPut("edit-video-news")]
        public ResponseBase EditVideoNews(EditVideoNewsDto videoNews)
        {
            var item = _newsServices.EditVideoNews(videoNews);
            return item;
        }
        [HttpPut("delete-video-news")]
        public ResponseBase DeleteVideoNews([FromBody] int Id)
        {
            var item = _newsServices.DeleteVideoNews(Id);
            return item;
        }
    }
}
