using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using GoStay.Data.Base;
using GoStay.Services;
using GoStay.Services.Newss;
using GoStay.DataDto.News;
using GoStay.DataAccess.Entities;
using System.Globalization;
using GoStay.Api.Attributes;
using GoStay.DataDto.Video;

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
        [Authorize]
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
        [Authorize]
        public ResponseBase SubmitNews(NewsDataDto newsDto)
        {
            var item = _newsServices.SubmitNews(newsDto);
            return item;
        }
        [HttpPut("update-status")]
        [Authorize]
        public ResponseBase UpdateStatusNews(UpdateStatusNewsParam param)
        {
            var item = _newsServices.UpdateStatusNews(param);
            return item;
        }
        [HttpPost("list")]
        //[Authorize]
        public ResponseBase GetListNews(GetListNewsParam param)
        {
            var items = _newsServices.GetListNews(param);
            return items;
        }
        [HttpGet("list-homepage")]
        [Authorize]
        public ResponseBase GetListNewsHomePage()
        {
            var items = _newsServices.GetListNewsHomePage();
            return items;
        }
        [HttpGet("tab-home")]
        [Authorize]
        public ResponseBase GetNewsForHomePage(int latestQuantity, int categoryQuantity, int hotQuantity, string dateStart, string dateEnd, int idcategory, int idtopic)
        {
            var start = DateTime.ParseExact(dateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(dateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var items = _newsServices.GetNewsForHomePage(latestQuantity, categoryQuantity, hotQuantity, start, end, idcategory, idtopic);
            return items;
        }
        [HttpGet("news")]
        public ResponseBase GetNews(int Id)
        {
            var items = _newsServices.GetNews(Id);
            return items;
        }
        [HttpGet("video-news")]
        [Authorize]
        public ResponseBase GetVideoNews(int Id)
        {
            var items = _newsServices.GetVideoNews(Id);
            return items;
        }

        [HttpGet("news-topic-total")]
        [Authorize]
        public ResponseBase GetNewsTopicTotal(int IdDomain)
        {
            var items = _newsServices.GetNewsTopicTotal(IdDomain);
            return items;
        }
        [HttpGet("news-category-total")]
        [Authorize]
        public ResponseBase GetNewsCategoryTotal(int IdDomain)
        {
            var items = _newsServices.GetNewsCategoryTotal(IdDomain);
            return items;
        }
        [HttpGet("list-category-by-parentid")]
        [Authorize]
        public ResponseBase GetListCategoryByParentId(int IdDomain, int ParentId)
        {
            var items = _newsServices.GetListCategoryByParentId(IdDomain, ParentId);
            return items;
        }

        [HttpPost("list-video-news")]
        [Authorize]
        public ResponseBase GetListVideoNews(GetListVideoNewsParam filter)
        {
            var items = _newsServices.GetListVideoNews(filter);
            return items;
        }


        [HttpGet("top-news")]
        [Authorize]
        public ResponseBase GetListTopNewsByCategory(int? IdCategory, int? IdTopic)
        {
            var items = _newsServices.GetListTopNewsByCategory(IdCategory, IdTopic);
            return items;
        }

        [HttpGet("data-support")]
        [Authorize]
        public ResponseBase GetDataSupportNews(int IdDoMain)
        {
            var items = _newsServices.GetDataSupportNews(IdDoMain);
            return items;
        }

        [HttpPost("add-news")]
        [Authorize]
        public ResponseBase AddNews(NewsDto newsDto)
        {
            var item = _newsServices.AddNews(newsDto);
            return item;
        }

        [HttpPost("add-video-news")]
        [Authorize]
        public ResponseBase AddVideoNews(VideoModel videoNews)
        {
            var item = _newsServices.AddVideoNews(videoNews);
            return item;
        }

        [HttpPut("edit-news")]
        [Authorize]
        public ResponseBase EditNews(NewsDto newsDto)
        {
            var item = _newsServices.EditNews(newsDto);
            return item;
        }

        [HttpPut("edit-content-news")]
        [Authorize]
        public ResponseBase EditContentNews(EditNewsContentParam param)
        {
            var item = _newsServices.EditContentNews(param.Content, param.NewsId);
            return item;
        }

        [HttpPut("edit-picturetitle-news")]
        [Authorize]
        public ResponseBase EditPictureTitleNews(EditNewsPictureTitleParam param)
        {
            var item = _newsServices.EditPictureTitleNews(param.Url, param.NewsId);
            return item;
        }
        [HttpPut("delete-news")]
        [Authorize]
        public ResponseBase DeleteNews([FromBody] int Id)
        {
            var item = _newsServices.DeleteNews(Id);
            return item;
        }
        [HttpPut("click")]
        [Authorize]
        public ResponseBase EditClickNews([FromBody] int Id)
        {
            var item = _newsServices.EditClickNews(Id);
            return item;
        }
        [HttpPut("edit-video-news")]
        [Authorize]
        public ResponseBase EditVideoNews(EditVideoNewsDto videoNews)
        {
            var item = _newsServices.EditVideoNews(videoNews);
            return item;
        }
        [HttpPut("delete-video-news")]
        [Authorize]
        public ResponseBase DeleteVideoNews([FromBody] int Id)
        {
            var item = _newsServices.DeleteVideoNews(Id);
            return item;
        }

        [HttpGet("hotel-near")]
        [Authorize]
        public ResponseBase GetNearHotel(int videoId)
        {
            var item = _newsServices.GetNearHotel(videoId);
            return item;
        }
        [HttpGet("default-video")]
        //[Authorize]
        public ResponseBase GetDefaultVideo(int idUser)
        {
            var item = _newsServices.GetDefaultVideo(idUser);
            return item;
        }
        [HttpGet("video-by-id")]
        //[Authorize]
        public ResponseBase GetVideoById(int id)
        {
            var item = _newsServices.GetVideoById(id);
            return item;
        }
        [HttpPost("upsert-video")]
        //[Authorize]
        public ResponseBase UpsertVideo(CreateVideoModel request)
        {
            var item = _newsServices.UpsertVideo(request);
            return item;
        }
        [HttpGet("tag-topic")]
        //[Authorize]
        public ResponseBase GetTagNews()
        {
            var item = _newsServices.GetTagNews();
            return item;
        }
        [HttpGet("tag-categories")]
        //[Authorize]
        public ResponseBase GetCategoryNews()
        {
            var item = _newsServices.GetCategoryNews();
            return item;
        }
        [HttpGet("news-cate-topic")]
        //[Authorize]
        public ResponseBase GetNewsByTopicAndCategory(int idCategory, int idTopic, int pageIndex, int pageSize)
        {
            var item = _newsServices.GetNewsByTopicAndCategory(idCategory, idTopic, pageIndex, pageSize);
            return item;
        }
    }
}
