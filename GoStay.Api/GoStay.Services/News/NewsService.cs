using AutoMapper;
using GoStay.Common;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.UnitOfWork;
using GoStay.DataDto.News;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using ErrorCodeMessage = GoStay.Data.Base.ErrorCodeMessage;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Services.Newss
{
    public class NewsService : INewsService
    {
        private readonly ICommonRepository<News> _newsRepository;
        private readonly ICommonRepository<NewsCategory> _newsCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonUoW _commonUoW;

        public NewsService(ICommonRepository<News> newsRepository, IMapper mapper, ICommonUoW commonUoW,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<NewsCategory> newsCategoryRepository)
        {
            _mapper = mapper;
            _pictureRepository = pictureRepository;
            _newsCategoryRepository = newsCategoryRepository;
            _newsRepository = newsRepository;
            _commonUoW = commonUoW;
        }
        public ResponseBase GetListNews(GetListNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {

                var listNews = NewsRepository.SearchListNews(param);
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = listNews;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseBase GetListNewsHomePage()
        {

            ResponseBase response = new ResponseBase();
            try
            {

                var dicNews = new Dictionary<int,List<NewSearchOutDto>>();
                var categories = _newsCategoryRepository.FindAll().Select(x=>x.Id);
                foreach(var Id in categories)
                {
                    dicNews.Add(Id,NewsRepository.SearchListNews(new GetListNewsParam { IdCategory = Id, PageIndex = 1, PageSize = 10 }));
                }
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = dicNews;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseBase GetNews(int Id)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var news = _newsRepository.FindAll(x=>x.Id == Id)
                            .Include(x=>x.IdCategoryNavigation)
                            .Include(x=>x.Pictures).Include(x=>x.IdUserNavigation)
                            .SingleOrDefault();
                if( news == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    return response;
                }
                var newsDetail = new NewsDetailDto()
                {
                    Id = news.Id,
                    IdCategory = news.IdCategory,
                    Status = news.Status,
                    IdUser = news.IdUser,
                    Title = news.Title,
                    Content = news.Content,
                    PictureTitle = news.PictureTitle,
                    Description = news.Description,
                    Category = news.IdCategoryNavigation.Category,
                    Pictures = news.Pictures.Select(x => x.UrlOut).ToList()

                };
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = newsDetail;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }

        public ResponseBase AddNews(NewsDto news)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = new News()
                {
                    Content = news.Content,
                    IdCategory = (int)news.IdCategory,
                    IdUser = (int)news.IdUser,
                    Status = (byte)news.Status,
                    Title = news.Title,
                    Keysearch = news.Keysearch,
                    Description = news.Description,
                    PictureTitle = news.PictureTitle,
                    DateCreate = DateTime.UtcNow,
                    DateEdit = DateTime.UtcNow,
                    Deleted = 0,
                };
                _newsRepository.Insert(newsEntity);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = newsEntity.Id;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseBase EditNews(NewsDto news)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _newsRepository.GetById(news.Id);
                if(newsEntity ==null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = news.Id;
                    return response;
                }
                newsEntity.IdCategory = (int)news.IdCategory;
                newsEntity.IdUser = (int)news.IdUser;
                newsEntity.Status = (byte)news.Status;
                newsEntity.Title = news.Title;
                newsEntity.Description = news.Description;
                newsEntity.Keysearch = news.Keysearch;
                newsEntity.DateEdit = DateTime.UtcNow;
                newsEntity.Deleted = 0;
                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = newsEntity.Id;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }

        public ResponseBase EditContentNews(string content, int NewsId)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _newsRepository.GetById(NewsId);
                if (newsEntity == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = NewsId;
                    return response;
                }
                newsEntity.Content = content;
                newsEntity.DateEdit = DateTime.UtcNow;

                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = newsEntity.Id;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }
        public ResponseBase EditPictureTitleNews(string url, int NewsId)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _newsRepository.GetById(NewsId);
                if (newsEntity == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = NewsId;
                    return response;
                }
                newsEntity.PictureTitle = url;
                newsEntity.DateEdit = DateTime.UtcNow;

                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = newsEntity.Id;
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }

        }

        public ResponseBase DeleteNews(int Id)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _newsRepository.GetById(Id);
                if (newsEntity == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = "Not found obj";
                    return response;
                }
                newsEntity.Deleted = 1;
                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = "Success";
                return response;

            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                response.Data = "Exception";

                return response;
            }

        }

        
    }
}
