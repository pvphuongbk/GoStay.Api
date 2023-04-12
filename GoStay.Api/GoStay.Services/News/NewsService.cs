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
        private readonly ICommonRepository<User> _userRepository;

        private readonly ICommonRepository<NewsCategory> _newsCategoryRepository;
        private readonly ICommonRepository<NewsTopic> _newsTopicRepository;
        private readonly ICommonRepository<TopicNews> _topicRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonUoW _commonUoW;

        public NewsService(ICommonRepository<News> newsRepository, IMapper mapper, ICommonUoW commonUoW,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<NewsCategory> newsCategoryRepository, ICommonRepository<User> userRepository
            , ICommonRepository<NewsTopic> newsTopicRepository, ICommonRepository<TopicNews> topicRepository)
        {
            _mapper = mapper;
            _pictureRepository = pictureRepository;
            _newsCategoryRepository = newsCategoryRepository;
            _newsRepository = newsRepository;
            _commonUoW = commonUoW;
            _userRepository = userRepository;
            _newsTopicRepository = newsTopicRepository;
            _topicRepository = topicRepository;
        }
        public ResponseBase GetListNews(GetListNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {

                var listNews = NewsRepository.SearchListNews(param);
                listNews.ForEach(x=>x.Slug=(x.Title.RemoveUnicode().Replace(" ", "-").Replace(",",string.Empty).Replace("--", string.Empty).ToLower()));
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
                    var data = NewsRepository.SearchListNews(new GetListNewsParam { IdCategory = Id, PageIndex = 1, PageSize = 10 });
                    data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()));
                    dicNews.Add(Id,data);
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
        public ResponseBase GetListTopNewsByCategory(int? IdCategory, int? IdTopic)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var data = NewsRepository.GetListTopNews(IdCategory, IdTopic);
                data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()));

                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = data;
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
                            .Include(x=>x.IdUserNavigation)
                            .Include(x=>x.NewsTopics).ThenInclude(y=>y.IdNewsTopicNavigation)
                            .Include(x=>x.Lang)
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
                    LangId = news.LangId,
                    DateCreate = news.DateCreate,
                    Language = news.Lang.Language1,
                    IdTopics = news.NewsTopics.Select(x => x.IdNewsTopic).ToList(),
                    Topics = news.NewsTopics.Select(x => x.IdNewsTopicNavigation.Topic).ToList(),
                    Tag = news.Tag,
                    UserName = news.IdUserNavigation.UserName,
                    Slug = news.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()
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
                    IdCategory = (int)news.IdCategory,
                    IdUser = (int)news.IdUser,
                    Status = 0,
                    Title = news.Title,
                    Keysearch = news.Keysearch,
                    Description = news.Description,
                    DateCreate = DateTime.UtcNow,
                    DateEdit = DateTime.UtcNow,
                    Deleted = 0,
                    LangId = (int)news.LangId,
                    PictureTitle = "",
                    Click = 0
                };
                
                _newsRepository.Insert(newsEntity);
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();

                if (news.IdTopics.Count > 0)
                {
                    foreach (var IdTopic in news.IdTopics)
                    {
                        _newsTopicRepository.Insert(new NewsTopic() { IdNews = newsEntity.Id, IdNewsTopic = IdTopic });

                    }
                }
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
                newsEntity.Title = news.Title;
                newsEntity.Description = news.Description;
                newsEntity.Keysearch = news.Keysearch;
                newsEntity.DateEdit = DateTime.UtcNow;
                newsEntity.LangId = (int)news.LangId;


                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                _newsTopicRepository.RemoveMultiple(_newsTopicRepository.FindAll(x=>x.IdNews==news.Id));

                if (news.IdTopics.Count > 0)
                {
                    foreach (var IdTopic in news.IdTopics)
                    {
                        _newsTopicRepository.Insert(new NewsTopic() { IdNews = newsEntity.Id, IdNewsTopic = IdTopic });

                    }
                }
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
        public ResponseBase EditClickNews(int NewsId)
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
                if (newsEntity.Click == null)
                {
                    newsEntity.Click = 0;
                }
                newsEntity.Click = newsEntity.Click+1;
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
