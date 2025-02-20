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
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using ErrorCodeMessage = GoStay.Data.Base.ErrorCodeMessage;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Services.Newss
{
    public partial class NewsService
    {

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

                var listCategories = _newsCategoryRepository.FindAll(x => x.Iddomain == 1).Select(x => x.Id).ToList();
                var list = NewsRepository.SearchListVideoNews(filter);
                list.ForEach(x => x.Slug = x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar());
                var data = new Dictionary<int, List<VideoNewsDto>>();
                
                data.Add(0, list);
                foreach (var item in listCategories)
                {
                    filter.IdCategory = item;
                    var dataCategory = NewsRepository.SearchListVideoNews(filter);
                    dataCategory.ForEach(x => x.Slug = x.Title.RemoveUnicode().ToLower().ReplaceSpecialChar());
                    data.Add(item, dataCategory);
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
                if (newsEntity != null)
                {
                    newsEntity.Video = news.Video;
                    newsEntity.IdCategory = news.IdCategory;
                    newsEntity.Title = news.Title;
                    newsEntity.Descriptions = news.Descriptions;

                    newsEntity.IdUser = news.IdUser;
                    newsEntity.PictureTitle = news.PictureTitle;
                    newsEntity.Name = news.Name;
                    newsEntity.KeySearch = newsEntity.Title.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                    newsEntity.Lon = news.Lon.HasValue? news.Lon:null;
                    newsEntity.Lat = news.Lat.HasValue ? news.Lat : null;

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
                    Lon = news.Lon.ToString().Replace(".", ","),
                    Lat = news.Lat.ToString().Replace(".", ",")
                };
                var quatityComment = _commentVideoRepo.FindAll(x => x.VideoId == news.Id && x.Published == true && x.Deleted == false).Count();
                newsDetail.QuatityComment = quatityComment;
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

        public ResponseBase GetDataSupportNews(int IdDoMain)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var cate = _newsCategoryRepository.FindAll(x => x.Iddomain == IdDoMain).ToList();
                var lan = _languageRepository.FindAll().ToList();
                var topic = _topicRepository.FindAll(x => x.Iddomain == IdDoMain).ToList();
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
                var topic = _topicRepository.FindAll(x => x.Iddomain == IdDomain);
                var newstopics = _newsTopicRepository.FindAll();

                foreach (var item in topic)
                {
                    var total = newstopics.Where(x => x.IdNewsTopic == item.Id).Count();
                    newsTopicTotals.Add(new NewsTopicTotal()
                    {
                        Id = item.Id,
                        Topic = item.Topic,
                        Total = total,
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
                var cate = _newsCategoryRepository.FindAll(x => x.Iddomain == IdDomain).Include(x => x.News.Where(y => y.Deleted != 1 && y.Iddomain == IdDomain));


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
        public ResponseBase GetNearHotel(int videoId)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var data = new List<HotelNearVideoDto>();

                var video = _videoRepository.GetById(videoId);
                if (video==null)
                {
                    response.Code = 400;
                    response.Message = "Không có video này";
                    response.Data = data;
                    return response;
                }
                if (!video.Lon.HasValue|| !video.Lat.HasValue|| video.Lon<=0||video.Lat<=0)
                {
                    response.Code = 400;
                    response.Message = "Không có tọa độ";
                    response.Data = data;
                    return response;
                }
                var lon = video.Lon.Value;
                var lat = video.Lat.Value;
                var hotels = _hotelRepo.FindAll(x => x.Deleted != 1 && (x.LonMap.HasValue && x.LatMap.HasValue))
                            .Select(x => new HotelNearDto
                            {
                                Id = x.Id,
                                Lon = x.LonMap.Value,
                                Lat = x.LatMap.Value,
                                Distance=0
                            }).ToList();
                hotels.ForEach(x =>
                {
                    x.Distance = Distance((double)lon, (double)lat, x.Lon, x.Lat);
                });
                hotels = hotels.OrderBy(x => x.Distance).Take(10).ToList();
                var listId = hotels.Select(x => x.Id);
                var listhotel = _hotelRepo.FindAll(x => listId.Contains(x.Id))
                                            .Include(x => x.HotelRooms.OrderBy(y => y.PriceValue).Take(1))
                                            .Include(x => x.Pictures.Take(3))
                                            .Include(x => x.IdTinhThanhNavigation)
                                            .Include(x => x.IdQuanNavigation).ToList();
                data = listhotel.Select(x => new HotelNearVideoDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Rating = x.Rating ?? 1,
                    Province = x.IdTinhThanhNavigation.TenTt ?? string.Empty,
                    District = x.IdQuanNavigation.Tenquan ?? string.Empty,
                    LastOrderTime = x.LastOrderTime,
                    Price = x.HotelRooms.FirstOrDefault().PriceValue,
                    Discount = x.HotelRooms.FirstOrDefault().Discount ?? 0,
                    Slug = x.Name.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower(),
                    Picture = x.Pictures.Select(y => y.Url).ToList(),
                    Distance = hotels.FirstOrDefault(z => z.Id==x.Id).Distance,
                }).ToList();
                var final = data.OrderBy(x => x.Distance).ToList();
                response.Data = final;
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
        static double Distance(double lon1, double lat1, double lon2, double lat2)
        {
            const double R = 6371; // Bán kính Trái Đất (km)
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            // Chuyển đổi sang radian
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            // Công thức Haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Khoảng cách (km)
        }

        static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
