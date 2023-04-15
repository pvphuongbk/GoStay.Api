﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("list-video-news")]
        public ResponseBase GetListVideoNews(int UserId, int status)
        {
            var items = _newsServices.GetListVideoNews(UserId,status);
            return items;
        }


        [HttpGet("top-news")]
        public ResponseBase GetListTopNewsByCategory(int? IdCategory, int? IdTopic)
        {
            var items = _newsServices.GetListTopNewsByCategory(IdCategory, IdTopic);
            return items;
        }

        [HttpPost("add-news")]
        public ResponseBase AddNews(NewsDto newsDto)
        {
            var item = _newsServices.AddNews(newsDto);
            return item;
        }

        [HttpPost("add-video-news")]
        public ResponseBase AddVideoNews(VideoNews videoNews)
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
    }
}
