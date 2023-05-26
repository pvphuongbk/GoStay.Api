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
using System.Security.Cryptography.X509Certificates;
using ErrorCodeMessage = GoStay.Data.Base.ErrorCodeMessage;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Services.Newss
{
    public class NewsService : INewsService
    {
        private readonly ICommonRepository<News> _newsRepository;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonRepository<Language> _languageRepository;

        private readonly ICommonRepository<NewsCategory> _newsCategoryRepository;
        private readonly ICommonRepository<NewsTopic> _newsTopicRepository;
        private readonly ICommonRepository<TopicNews> _topicRepository;
        private readonly ICommonRepository<VideoNews> _videoRepository;

        private readonly IMapper _mapper;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonUoW _commonUoW;

        public NewsService(ICommonRepository<News> newsRepository, IMapper mapper, ICommonUoW commonUoW,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<NewsCategory> newsCategoryRepository, ICommonRepository<User> userRepository
            , ICommonRepository<NewsTopic> newsTopicRepository, ICommonRepository<TopicNews> topicRepository, ICommonRepository<VideoNews> videoRepository, ICommonRepository<Language> languageRepository)
        {
            _mapper = mapper;
            _pictureRepository = pictureRepository;
            _newsCategoryRepository = newsCategoryRepository;
            _newsRepository = newsRepository;
            _commonUoW = commonUoW;
            _userRepository = userRepository;
            _newsTopicRepository = newsTopicRepository;
            _topicRepository = topicRepository;
            _videoRepository = videoRepository;
            _languageRepository = languageRepository;
        }
        public ResponseBase GetListNews(GetListNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {

                var listNews = NewsRepository.SearchListNews(param);
                listNews.ForEach(x=>x.Slug=(x.Title.RemoveUnicode().Replace(" ", "-").Replace(",",string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty)
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty)

                                            .ToLower()));
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
                    var data = NewsRepository.SearchListNews(new GetListNewsParam { IdCategory = Id,IdDomain=1, PageIndex = 1, PageSize = 10 });
                    data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
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
                data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));

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
                    Slug = news.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()
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
                    Click = 0,
                    Iddomain= news.IdDomain
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
                newsEntity.Iddomain = (int)news.IdDomain;



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
                var listnewstopics = _newsTopicRepository.FindAll(x => x.IdNews == newsEntity.Id);
                _newsTopicRepository.RemoveMultiple(listnewstopics);
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
        public ResponseBase GetListVideoNews(GetListVideoNewsParam filter)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                if (filter.TextSearch == null)
                {
                    filter.TextSearch = "";
                }
                filter.TextSearch = filter.TextSearch.RemoveUnicode();
                filter.TextSearch = filter.TextSearch.Replace(" ", string.Empty).ToLower();

                var list = NewsRepository.SearchListVideoNews(filter);
                list.ForEach(x=>x.Slug = x.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower());
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = list;
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
        public ResponseBase AddVideoNews(VideoModel videonews)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var news = _mapper.Map<VideoModel, VideoNews>(videonews);
                news.DateCreate = DateTime.Now;
                _commonUoW.BeginTransaction();
                news.Status = 1;
                if (news.Title == null)
                    news.Title = $"Video {DateTime.Now.ToString("dd/MM/yyyy")}";
                news.KeySearch = news.Title.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                _videoRepository.Insert(news);
                _commonUoW.Commit();
                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = news.Id;
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
        public ResponseBase EditVideoNews(EditVideoNewsDto news)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _videoRepository.GetById(news.Id);
                if(newsEntity != null)
                {
                    newsEntity.Video = news.Video;
                    newsEntity.IdCategory = news.IdCategory;
                    newsEntity.Title = news.Title;
                    newsEntity.Descriptions = news.Descriptions;

                    newsEntity.IdUser = news.IdUser;
                    newsEntity.PictureTitle = news.PictureTitle;
                    newsEntity.Name = news.Name;
                    newsEntity.KeySearch = newsEntity.Title.RemoveUnicode().Replace(" ", string.Empty).ToLower();

                }
                newsEntity.Status = 1;
                _videoRepository.Update(newsEntity);
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
        public ResponseBase DeleteVideoNews(int Id)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var newsEntity = _videoRepository.GetById(Id);
                if (newsEntity == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = "Not found obj";
                    return response;
                }
                newsEntity.Deleted = 1;
                _videoRepository.Update(newsEntity);
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
        public ResponseBase GetVideoNews(int Id)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var news = _videoRepository.FindAll(x => x.Id == Id)
                            .Include(x => x.IdCategoryNavigation)
                            .Include(x => x.IdUserNavigation)
                            .Include(x => x.Lang)
                            .SingleOrDefault();
                if (news == null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    return response;
                }
                var newsDetail = new VideoNewsDetailDto()
                {
                    Id = news.Id,
                    IdCategory = news.IdCategory,
                    Status = news.Status,
                    IdUser = news.IdUser,
                    Title = news.Title,
                    Video = news.Video,
                    Descriptions = news.Descriptions,
                    PictureTitle = news.PictureTitle,
                    Category = news.IdCategoryNavigation.Category,
                    LangId = news.LangId,
                    DateCreate = news.DateCreate,
                    Language = news.Lang.Language1,
                    UserName = news.IdUserNavigation.UserName,
                    Click = news.Click,
                };
                newsDetail.Avatar = news.IdUserNavigation.Picture;
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

        public ResponseBase GetDataSupportNews()
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var cate = _newsCategoryRepository.FindAll(x=>x.Iddomain==2).ToList();
                var lan = _languageRepository.FindAll().ToList();
                var topic = _topicRepository.FindAll(x => x.Iddomain == 2).ToList();
                DataSupportNews data = new DataSupportNews()
                {
                    ListCategory = cate,
                    ListLanguage = lan,
                    ListTopic = topic
                };
                responseBase.Data = data;
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                return responseBase;
            }
        }
        public ResponseBase GetNewsTopicTotal(int IdDomain)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<NewsTopicTotal> newsTopicTotals = new List<NewsTopicTotal>();
                var topic = _topicRepository.FindAll(x=>x.Iddomain == IdDomain);
                var newstopics = _newsTopicRepository.FindAll();

                foreach (var item in topic)
                {
                    var total = newstopics.Where(x=>x.IdNewsTopic==item.Id).Count();
                    newsTopicTotals.Add(new NewsTopicTotal() { Id = item.Id, Topic = item.Topic,Total = total,
                        Slug = item.Topic.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()
                    });
                };

                responseBase.Data = newsTopicTotals;
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                return responseBase;
            }
        }
        public ResponseBase GetNewsCategoryTotal(int IdDomain)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<NewsCategoryTotal> newsTopicTotals = new List<NewsCategoryTotal>();
                var cate = _newsCategoryRepository.FindAll(x => x.Iddomain == IdDomain).Include(x=>x.News);

                foreach (var item in cate)
                {
                    newsTopicTotals.Add(new NewsCategoryTotal()
                    {
                        Id = item.Id,
                        Category = item.Category,
                        Total = item.News.Count(),
                        Slug = item.Category.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()
                    });
                };

                responseBase.Data = newsTopicTotals;
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception ex)
            {
                responseBase.Message = ex.Message;
                return responseBase;
            }
        }
    }
}
