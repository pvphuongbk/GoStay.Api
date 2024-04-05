using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.UnitOfWork;
using GoStay.DataDto.News;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.IdentityModel.Tokens;
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
        public ResponseBase GetNewsDefault(int idUser, int idNews)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var data = new NewsParamDto();
                var idRecord = 0;
                var exception = "";
                var categories = _newsCategoryRepository.FindAll(x=>x.Iddomain== AppConfigs.IdDomain).Select(x=> new NewsCategoryDataDto
                {
                    Id = x.Id,
                    Category = x.Category,
                    CategoryChi = x.CategoryChi,
                    CategoryEng = x.CategoryEng,
                }).ToList();
                data.Categories = categories;
                var topics = _topicRepository.FindAll(x => x.Iddomain==AppConfigs.IdDomain).Select(x => new TopicNewsDataDto
                {
                    Id=x.Id,
                    Topic = x.Topic,
                }).ToList();
                data.Topics = topics;
                var tempRecord = _newsRepository.FindAll(x => x.IdUser == idUser && x.Status==0).AsNoTracking();

                if(tempRecord.Any())
                {
                    var tempRecordIds = tempRecord.Select(x => x.Id);
                    if (tempRecord.Count()>1)
                    {
                        
                        //topics
                        var oldTopics = _newsTopicRepository.FindAll(x=>tempRecordIds.Contains(x.IdNews));
                        if (oldTopics.Any())
                        {
                            try
                            {
                                _commonUoW.BeginTransaction();
                                _newsTopicRepository.RemoveMultiple(oldTopics);
                                _commonUoW.Commit();
                            }
                            catch
                            {
                                exception += "Remove old topic fail";
                            }
                        }
                        //pictures
                        var oldPicture = _pictureRepository.FindAll(x => tempRecordIds.Contains((x.NewsId!=null)?(int)x.NewsId:0));

                        if (oldPicture.Any())
                        {
                            //FileHelper.DeleteFolder($"/uploads/SGOland/ctv/{newsEntity.IdUser}/news/{Id}");
                            try
                            {
                                _commonUoW.BeginTransaction();
                                _pictureRepository.RemoveMultiple(oldPicture);
                                _commonUoW.Commit();
                            }
                            catch
                            {
                                exception += "Remove old pictures fail";
                            }
                        }
                        try
                        {
                            idRecord=0;
                            _commonUoW.BeginTransaction();
                            _newsRepository.RemoveMultiple(tempRecord);
                            _commonUoW.Commit();
                        }
                        catch
                        {
                            exception += "Remove old news fail";
                        }
                    }
                    else
                    {
                        idRecord=tempRecord.SingleOrDefault().Id;
                    }
                }
                if (idNews > 0)
                {
                    idRecord = idNews;
                }
                if(idRecord==0)
                {
                    var newRecord = new News()
                    {
                        IdCategory = categories.Min(x => x.Id),
                        IdUser = idUser,
                        DateCreate = DateTime.Now,
                        DateEdit = DateTime.Now,
                        Status = 0,
                        Keysearch = "",
                        Title = "",
                        Description = "",
                        Content = "",
                        Deleted = 0,
                        LangId=1,
                        Click = 0,
                        Tag = "",
                        Iddomain = AppConfigs.IdDomain,

                    };

                    try
                    {
                        _commonUoW.BeginTransaction();
                        _newsRepository.Insert(newRecord);
                        _commonUoW.Commit();
                        idRecord = newRecord.Id;
                    }
                    catch
                    {
                        exception += "Add temp news fail";
                    }
                }

                var news = _newsRepository.FindAll(x=>x.Id==idRecord)
                                            .Include(x=>x.NewsTopics)
                                            .Include(x=>x.IdUserNavigation)
                                            .SingleOrDefault()
                                            ;
                data.News = new NewsDataDto()
                {
                    Id = news.Id,
                    IdCategory = news.IdCategory,
                    IdUser = news.IdUser,
                    Keysearch= news.Keysearch,
                    Title = news.Title,
                    Description = news.Description,
                    LangId = news.LangId,
                    IdDomain = news.Iddomain,
                    PictureTitle =(idNews>0) ? news.PictureTitle:"",
                    DateCreate = news.DateCreate,
                    Topics = (idNews > 0) ? topics.Where(x => news.NewsTopics.Select(y => y.IdNewsTopic).Contains(x.Id)).ToList(): new List<TopicNewsDataDto>(),
                    TopicIds = (idNews > 0) ? topics.Where(x => news.NewsTopics.Select(y => y.IdNewsTopic).Contains(x.Id)).Select(x=>x.Id).ToList(): new List<int>(),
                    TopicValues = (idNews > 0) ? topics.Where(x => news.NewsTopics.Select(y => y.IdNewsTopic).Contains(x.Id)).Select(x => x.Topic).ToList(): new List<string>(),
                    UserData = new UserDataDto()
                    {
                        UserId = news.IdUser,
                        FirstName = news.IdUserNavigation.FirstName,
                        LastName = news.IdUserNavigation.LastName,
                    },
                    Content = news.Content,
                    Status = news.Status,
                    Category = categories.SingleOrDefault(x => x.Id==news.IdCategory),
                    Slug = news.Title.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty)
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()
                };
                
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
        public ResponseBase GetListNews2(GetListNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var data = new List<NewsDataDto>();
                if (AppConfigs.AdminIds.Contains(param.UserId??0))
                {
                    param.UserId=0;
                }
                var listNews = _newsRepository.FindAll(x=>x.Deleted!=1
                                                        &&((param.UserId>0) ? x.IdUser==param.UserId : x.Id>0)
                                                        &&((param.Status>0)?x.Status==param.Status:x.Status>0)
                                                        &&((param.IdCategory>0) ? x.IdCategory==param.IdCategory : x.IdCategory>0)
                                                        &&((param.IdTopic>0) ? x.NewsTopics.Select(y=>y.IdNewsTopic).Contains((int)param.IdTopic) : x.Id>0)
                                                        &&((param.IdDomain>0) ? x.Iddomain==param.IdDomain : x.Iddomain==AppConfigs.IdDomain)
                                                        &&((param.TextSearch!=null) ? x.Keysearch.Contains(param.TextSearch) : x.Id>0))
                                                .Include(x=>x.IdCategoryNavigation)
                                                .Include(x=>x.NewsTopics).ThenInclude(x=>x.IdNewsTopicNavigation)
                                                .Include(x=>x.IdUserNavigation).OrderBy(x=>x.Status).ThenByDescending(x=>x.DateEdit).AsNoTracking();
                var count = listNews.Count(); 
                var pageSize = param.PageSize;
                var pageIndex = param.PageIndex;
                if(pageIndex> ((count-1)/pageSize+1))
                {
                    response.Code = ErrorCodeMessage.OutRange.Key;
                    response.Message = ErrorCodeMessage.OutRange.Value;
                    response.Data = listNews;
                    return response;
                }
                data = listNews.Select(x=> new NewsDataDto
                {
                    Id = x.Id,
                    IdCategory = x.IdCategory,
                    IdUser = x.IdUser,
                    Keysearch= x.Keysearch,
                    Title = x.Title,
                    Description = x.Description,
                    LangId = x.LangId,
                    PictureTitle = x.PictureTitle,
                    DateCreate = x.DateCreate,
                    IdDomain = x.Iddomain,
                    Status = x.Status,
                    Content = x.Content,
                    Topics = x.NewsTopics.Select(z=>z.IdNewsTopicNavigation).Select(y =>new TopicNewsDataDto
                    {
                        Id=y.Id,
                        Topic = y.Topic,
                    }).ToList(),
                    UserData = new UserDataDto()
                    {
                        UserId = x.IdUser,
                        FirstName = x.IdUserNavigation.FirstName,
                        LastName = x.IdUserNavigation.LastName,
                    },
                    Category = new NewsCategoryDataDto
                    {
                        Id = x.IdCategory,
                        Category = x.IdCategoryNavigation.Category,
                        CategoryChi = x.IdCategoryNavigation.CategoryChi,
                        CategoryEng = x.IdCategoryNavigation.CategoryEng,
                    },
                    Slug = x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar()
                }).Skip(pageSize*(pageIndex-1)).Take(pageSize).ToList();


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
        public ResponseBase GetListNews(GetListNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {

                var listNews = NewsRepository.SearchListNews(param);
                listNews.ForEach(x=>x.Slug=(x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar()));
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
        public ResponseBase SubmitNews(NewsDataDto news)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var topics = _topicRepository.FindAll(x => x.Iddomain == AppConfigs.IdDomain).Select(x => new TopicNewsDataDto
                {
                    Id = x.Id,
                    Topic = x.Topic,
                }).ToList();
                _commonUoW.BeginTransaction();
                var newsEntity = _newsRepository.GetById(news.Id);
                if (newsEntity ==null)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = "Not found";
                    return response;
                }
                newsEntity.IdCategory = (int)news.IdCategory;
                newsEntity.Title = news.Title;
                newsEntity.Description = news.Description;
                newsEntity.Keysearch = news.Title.RemoveUnicode().Replace(" ",string.Empty).ToLower();
                newsEntity.DateEdit = DateTime.UtcNow;
                newsEntity.LangId = (int)news.LangId;
                newsEntity.Iddomain = AppConfigs.IdDomain;
                newsEntity.Content = news.Content;
                newsEntity.Status = news.Status;
                if (news.TopicValues.Any())
                {
                    news.TopicIds = topics.Where(x => news.TopicValues.Contains(x.Topic)).Select(x => x.Id).ToList();
                }
                _newsRepository.Update(newsEntity);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();
                var oldtopic = _newsTopicRepository.FindAll(x => x.IdNews==news.Id);
                if(oldtopic.Any())
                {
                    _newsTopicRepository.RemoveMultiple(oldtopic);
                    _commonUoW.Commit();
                    _commonUoW.BeginTransaction();

                }

                if (news.TopicIds.Any())
                {
                    var newTopic = news.TopicIds.Select(x => new NewsTopic
                    {
                        IdNews = newsEntity.Id,
                        IdNewsTopic = x
                    });
                    _newsTopicRepository.InsertMultiple(newTopic);
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
        public ResponseBase UpdateStatusNews(UpdateStatusNewsParam param)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var data = _newsRepository.FindAll(x => x.Id==param.Id).SingleOrDefault();
                if(data==null)
                {
                    response.Code= ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;
                    response.Data = 0;
                    return response;
                }
                data.Status = param.Status;
                _commonUoW.BeginTransaction();
                
                _commonUoW.Commit();

                response.Code = ErrorCodeMessage.Success.Key;
                response.Message = ErrorCodeMessage.Success.Value;
                response.Data = 3;
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
        public ResponseBase GetNewsForHomePage(int latestQuantity,int categoryQuantity,int hotQuantlty, DateTime dateStart, DateTime dateEnd)
        {

            ResponseBase response = new ResponseBase();
            var data = new NewsTabHome()
            {
                HotNews = new List<NewsHomeData>(),
                LatestNews = new List<NewsHomeData>(),
                NewsForCategories = new Dictionary<int, List<NewsHomeData>>()
            };
            try
            {
                var allnews = _newsRepository.FindAll(x=>x.Status== (int)NewsStatus.Accepted &&x.Deleted!=1 &&x.Iddomain==AppConfigs.IdDomain)
                                             .Include(x=>x.IdCategoryNavigation)
                                             .Include(x=>x.IdUserNavigation)
                                             .Include(x=>x.NewsTopics).ThenInclude(y=>y.IdNewsTopicNavigation)
                                             .AsNoTracking();
                var temp = allnews.Select(x => new NewsHomeData
                {
                    Id = x.Id,
                    Status = x.Status,
                    Title = x.Title,
                    DateCreate = x.DateEdit,
                    IdCategory = x.IdCategory,
                    PictureTitle = x.PictureTitle,
                    Description = x.Description,
                    Category = x.IdCategoryNavigation.Category,
                    CategoryChi = x.IdCategoryNavigation.CategoryChi,
                    CategoryEng = x.IdCategoryNavigation.CategoryEng,
                    UserName = x.IdUserNavigation.UserName,
                    Click = x.Click??0,
                    Slug = (!x.Title.IsNullOrEmpty()) ? x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar() : ""
                });

                data.LatestNews = temp.OrderByDescending(x=>x.DateCreate).Take(latestQuantity).ToList();

                data.HotNews = temp.Where(x=>x.DateCreate>=dateStart && x.DateCreate<=dateEnd).OrderByDescending(x=>x.Click).Take(hotQuantlty).ToList();

                data.Categories = _newsCategoryRepository.FindAll(x => x.Iddomain==AppConfigs.IdDomain)
                                    .Select(x=> new CategoryNews
                                    {
                                        Id= x.Id,
                                        Name = x.Category
                                    }).ToList();
                var temp2 = data.Categories.Select(x=>x.Id);

                foreach (var cate in temp2)
                {
                    var news = temp.Where(x=>x.IdCategory==cate).OrderByDescending(x => x.DateCreate).Take(categoryQuantity).ToList();
                    data.NewsForCategories.Add(cate, news);
                }

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
                    data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar()));
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
                data.ForEach(x => x.Slug = (x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar()));

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
                    Slug = news.Title.RemoveUnicode().ToLower().ReplaceSpecialChar()
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
                list.ForEach(x=>x.Slug = x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar());
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
                var cate = _newsCategoryRepository.FindAll(x => x.Iddomain == IdDomain).Include(x=>x.News.Where(y=>y.Deleted!=1));

                foreach (var item in cate)
                {
                    newsTopicTotals.Add(new NewsCategoryTotal()
                    {
                        Id = item.Id,
                        Category = item.Category,
                        Total = item.News.Count(),
                        Slug = item.Category.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower(),
                        CategoryChi = item.CategoryChi,
                        CategoryEng = item.CategoryEng,
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
        public ResponseBase GetListCategoryByParentId(int IdDomain, int ParentId)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<NewsCategoryTotal> newsTopicTotals = new List<NewsCategoryTotal>();
                var cate = _newsCategoryRepository.FindAll(x => x.Iddomain == IdDomain && x.ParentId == ParentId).Include(x => x.News.Where(y => y.Deleted != 1));

                foreach (var item in cate)
                {
                    newsTopicTotals.Add(new NewsCategoryTotal()
                    {
                        Id = item.Id,
                        Category = item.Category,
                        Total = item.News.Count(),
                        Slug = item.Category.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower(),
                        CategoryChi = item.CategoryChi,
                        CategoryEng = item.CategoryEng,
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
