using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Services.Comments;

public class CommentService : ICommentService
{
    private readonly ICommonRepository<News> _newsRepository;
    private readonly ICommonRepository<User> _userRepository;
    private readonly ICommonRepository<Language> _languageRepository;

    private readonly ICommonRepository<NewsCategory> _newsCategoryRepository;
    private readonly ICommonRepository<NewsTopic> _newsTopicRepository;
    private readonly ICommonRepository<TopicNews> _topicRepository;
    private readonly ICommonRepository<VideoNews> _videoRepository;
    private readonly ICommonRepository<CommentNews> _commentNewsRepo;
    private readonly ICommonRepository<CommentVideo> _commentVideoRepo;


    private readonly IMapper _mapper;
    private readonly ICommonRepository<Picture> _pictureRepository;
    private readonly ICommonUoW _commonUoW;

    public CommentService(ICommonRepository<News> newsRepository, IMapper mapper, ICommonUoW commonUoW,
        ICommonRepository<Picture> pictureRepository, ICommonRepository<NewsCategory> newsCategoryRepository, ICommonRepository<User> userRepository
        , ICommonRepository<NewsTopic> newsTopicRepository, ICommonRepository<TopicNews> topicRepository, ICommonRepository<VideoNews> videoRepository, 
        ICommonRepository<Language> languageRepository, ICommonRepository<CommentNews> commentNewsRepo, ICommonRepository<CommentVideo> commentVideoRepo)
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
        _commentNewsRepo = commentNewsRepo;
        _commentVideoRepo = commentVideoRepo;
    }
    //news
    public ResponseBase UpsertCommentNews(CommentNewsUpsertRequestModel request)
    {
        var response = new ResponseBase();
        try
        {
            var comment = new CommentNews()
            {
                CreatedDate = DateTime.Now,
            };

            var news = _newsRepository.GetById(request.NewsId);
            if (news == null) 
            {
                response.Message = "News not existing";
                return response;
            }
            var user = _userRepository.GetById(request.UserId);
            if (user == null)
            {
                response.Message = "Login now";
                return response;
            }
            if (request.Id > 0)
            {
                comment = _commentNewsRepo.GetById(request.Id);

                if (comment == null)
                    comment = new CommentNews()
                    {
                        CreatedDate = DateTime.Now,
                    }; 
            }
            comment.NewsId = request.NewsId;
            comment.UserId = request.UserId;
            comment.Content = request.Content;
            comment.ParentId = request.ParentId;
            comment.ModifiedDate = DateTime.Now;
            comment.Published = false;
            comment.Deleted = false;

            _commonUoW.BeginTransaction();
            _commentNewsRepo.Update(comment);
            _commonUoW.Commit();
            response.Data = comment.Id;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase DeleteCommentNews(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentNewsRepo.GetById(id);
            if(comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            _commonUoW.BeginTransaction();
            _commentNewsRepo.Remove(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";
            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase DraftCommentNews(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentNewsRepo.GetById(id);
            if (comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            comment.Deleted = true;
            _commonUoW.BeginTransaction();
            _commentNewsRepo.Update(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";
            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase PublishCommentNews(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentNewsRepo.GetById(id);
            if (comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            comment.Published = !comment.Published;
            _commonUoW.BeginTransaction();
            _commentNewsRepo.Update(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";

            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase GetCommentNews(int userId, int newsId, int pageIndex, int pageSize, List<CommentChildRequestModel>? listChildRequest)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var result = new CommentNewsResponseModel();
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                result.UserName = "";
                result.UserAvatar = "";
            }
            else
            {
                result.UserName = user.UserName;
                result.UserAvatar = user.Picture;
            }
            result.UserId = userId;
            var comments = new List<CommentNewsViewModel>();
            var news = _newsRepository.GetById(newsId);
            if (news == null)
            {
                response.Message = "News not existing";
                return response;
            }
            var totalQuantity = _commentNewsRepo.FindAll(x => x.NewsId == newsId && x.ParentId == 0 && x.Deleted == false
                                                        && ((x.UserId!=userId && x.Published == true)||(x.UserId == userId) )).Count();
            if (totalQuantity == 0)
            {
                result.ListComment = comments;

            }
            else
            {
                comments = _commentNewsRepo.FindAll(x => x.NewsId == newsId && x.ParentId == 0 && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId)))
                            .Include(x => x.User).OrderByDescending(x => x.CreatedDate).Take(pageIndex * pageSize).Select(x => new CommentNewsViewModel
                            {
                                Comment = new CommentNewsModel
                                {
                                    Id = x.Id,
                                    NewsId = x.NewsId,
                                    UserId = x.UserId,
                                    Username = x.User.UserName,
                                    UserAvatar = x.User.Picture,
                                    Content = x.Content,
                                    ParentId = x.ParentId,
                                    CreatedDate = x.CreatedDate,
                                    ModifiedDate = x.ModifiedDate,
                                    Published = x.Published,
                                },
                                ReplyComments = new()
                            }).ToList();
                if (!comments.Any())
                {
                    return response;
                }
                var listParentId = comments.Select(x => x.Comment.Id).ToList();
                var listReply = _commentNewsRepo.FindAll(x => listParentId.Contains(x.ParentId) && x.Deleted == false
                                                    && ((x.UserId != userId && x.Published == true) || (x.UserId == userId))).Include(x => x.User).ToList();

                foreach (var item in comments)
                {
                    var rep = listReply.Where(x => x.ParentId == item.Comment.Id).OrderByDescending(x => x.CreatedDate);
                    var totalquantity = rep.Count();
                    if (totalquantity == 0)
                        continue;
                    var childRequest = listChildRequest?.SingleOrDefault(x => x.ParentId == item.Comment.Id);
                    var childQuantity = (childRequest != null ? childRequest.PageIndex : 1) * (childRequest != null ? childRequest.PageSize : 5);
                    item.ReplyComments.ReplyComments = rep.Take(childQuantity).Select(x => new CommentNewsModel
                    {
                        Id = x.Id,
                        NewsId = x.NewsId,
                        UserId = x.UserId,
                        Username = x.User.UserName,
                        UserAvatar = x.User.Picture,
                        Content = x.Content,
                        ParentId = x.ParentId,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate,
                        Published = x.Published,
                    }).ToList();
                    item.ReplyComments.ParentCommentId = item.Comment.Id;
                    item.ReplyComments.TotalQuantityRep = totalquantity;
                    item.ReplyComments.CurentQuantityRep = 5;
                }
                result.ListComment = comments;
                result.TotalQuantity = totalQuantity;
                result.CurentQuantity = pageIndex * pageSize;
            }
            response.Data = result;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase GetCommentReplyNews(int userId, int parentCommentId, int pageIndex, int pageSize)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var result = new CommentReplyNewsModel()
            {
                ParentCommentId = parentCommentId,
                TotalQuantityRep = 0,
                CurentQuantityRep = 0,
            };

            var quantity = _commentNewsRepo.FindAll(x => x.ParentId == parentCommentId && x.Deleted == false 
                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId)) ).Count();
            if (quantity == 0)
            {
                response.Data = result;
                return response;
            }

            var listReply = _commentNewsRepo.FindAll(x => x.ParentId == parentCommentId && x.Deleted == false
                                                    && ((x.UserId != userId && x.Published == true) || (x.UserId == userId)))
                                                .Include(x => x.User)
                                                .OrderByDescending(x => x.CreatedDate).Take(pageIndex * pageSize)
                                                .ToList();
            var data = listReply.Select(x => new CommentNewsModel
            {
                Id = x.Id,
                NewsId = x.NewsId,
                UserId = x.UserId,
                Username = x.User.UserName,
                UserAvatar = x.User.Picture,
                Content = x.Content,
                ParentId = x.ParentId,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Published = x.Published
            }).ToList();
            result.TotalQuantityRep = quantity;
            result.CurentQuantityRep = pageIndex * pageSize;
            result.ReplyComments = data;
            response.Data = result;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public async Task<ResponseBase> GetCommentNewsForApproval(string? newsTitle,bool? publish,int? categoryId,int? topicId, int pageIndex, int pageSize)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var data = new CommentNewsForApproval()
            {
                ListData = new List<CommentNewsData>(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Quantity = 0,
            };
            var listNewsTopic = new List<int>();
            if(topicId.HasValue)
            {
                listNewsTopic = _newsTopicRepository.FindAll(x=>x.IdNewsTopic == topicId).Select(x=>x.IdNews).ToList();
            }    
            var listNews = _newsRepository.FindAll(x=> (!string.IsNullOrEmpty(newsTitle)?
                                                            x.Keysearch.Contains(newsTitle):
                                                            (
                                                                (categoryId.HasValue?x.IdCategory==categoryId:true)
                                                              &&(topicId.HasValue? listNewsTopic.Contains(x.Id):true)
                                                              )
                                                        )
                                                    )
                                                    .Include(x=>x.NewsTopics)
                                                    .Select(x=>new
                                                    {
                                                        Id = x.Id,
                                                        Title = x.Title,
                                                    }).ToList();

            var listNewsIds = listNews.Select(x=>x.Id).ToList();

            var quantityComment =await _commentNewsRepo.CountWhere(x => listNewsIds.Contains(x.NewsId)&& (publish.HasValue?x.Published==publish:true)&& x.Deleted==false);

            if((pageIndex-1)*pageSize > quantityComment)
            {
                response.Data = data;
                return response;
            }
            data.Quantity=quantityComment;
            var comments = _commentNewsRepo.FindAll(x => listNewsIds.Contains(x.NewsId) && (publish.HasValue ? x.Published == publish : true) && x.Deleted == false)
                                            .OrderByDescending(x=>x.ModifiedDate)
                                            .Skip((pageIndex-1)*pageSize).Take(pageSize)
                                            .Select(x=> new CommentNewsDetail
                                            {
                                                Id=x.Id,
                                                UserId = x.UserId,
                                                NewsId = x.NewsId,
                                                Content = x.Content,
                                                ParentId = x.ParentId,
                                                Publish = x.Published,
                                                CreatedDate = x.CreatedDate,
                                                ModifiedDate = x.ModifiedDate,
                                            }).ToList();
            data.ListData = comments.GroupBy(x=>x.NewsId).Select(x=>new CommentNewsData
            {
                NewsId = x.Key,
                NewsTitle = listNews.FirstOrDefault(y=>y.Id==x.Key).Title,
                ListComment = x.ToList()
            }).ToList();
            response.Code = 200;
            response.Data = data;
            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }

    //video
    public ResponseBase UpsertCommentVideo(CommentVideoUpsertRequestModel request)
    {
        var response = new ResponseBase();
        try
        {
            var comment = new CommentVideo()
            {
                CreatedDate = DateTime.Now,
            };
            var video = _videoRepository.GetById(request.VideoId);
            if (video == null)
            {
                response.Message = "Video not existing";
                return response;
            }
            var user = _userRepository.GetById(request.UserId);
            if (user == null)
            {
                response.Message = "Login now";
                return response;
            }
            if (request.Id > 0)
            {
                comment = _commentVideoRepo.GetById(request.Id);

                if (comment == null)
                    comment = new CommentVideo()
                    {
                        CreatedDate = DateTime.Now,
                    };
            }
            comment.VideoId = request.VideoId;
            comment.UserId = request.UserId;
            comment.Content = request.Content;
            comment.ParentId = request.ParentId;
            comment.ModifiedDate = DateTime.Now;
            comment.Published = false;
            comment.Deleted = false;

            _commonUoW.BeginTransaction();
            _commentVideoRepo.Update(comment);
            _commonUoW.Commit();
            response.Data = comment.Id;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase DeleteCommentVideo(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentVideoRepo.GetById(id);
            if (comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            _commonUoW.BeginTransaction();
            _commentVideoRepo.Remove(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";

            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase DraftCommentVideo(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentVideoRepo.GetById(id);
            if (comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            comment.Deleted=true;
            _commonUoW.BeginTransaction();
            _commentVideoRepo.Update(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";

            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase PublishCommentVideo(int id)
    {
        var response = new ResponseBase();
        try
        {
            var comment = _commentVideoRepo.GetById(id);
            if (comment == null)
            {
                response.Code = 400;
                response.Message = "Không có comment này";
                return response;
            }
            comment.Published = !comment.Published;
            _commonUoW.BeginTransaction();
            _commentVideoRepo.Update(comment);
            _commonUoW.Commit();
            response.Code = 200;
            response.Data = "Success";

            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase GetCommentVideo(int userId, int videoId, int pageIndex, int pageSize, List<CommentChildRequestModel>? listChildRequest)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var result = new CommentVideoResponseModel();
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                result.UserName = "";
                result.UserAvatar = "";
            }
            else
            {
                result.UserName = user.UserName;
                result.UserAvatar = user.Picture;
            }
            result.UserId = userId;
            var comments = new List<CommentVideoViewModel>();
            var news = _videoRepository.GetById(videoId);
            if (news == null)
            {
                response.Message = "Video not existing";
                return response;
            }
            var totalQuantity = _commentVideoRepo.FindAll(x => x.VideoId == videoId && x.ParentId == 0 && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId))).Count();
            if (totalQuantity == 0)
            {
                result.ListComment = comments;
            }
            else
            {
                comments = _commentVideoRepo.FindAll(x => x.VideoId == videoId && x.ParentId == 0 && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId)))
                            .Include(x => x.User).OrderByDescending(x => x.CreatedDate).Take(pageIndex * pageSize).Select(x => new CommentVideoViewModel
                            {
                                Comment = new CommentVideoModel
                                {
                                    Id = x.Id,
                                    VideoId = x.VideoId,
                                    UserId = x.UserId,
                                    Username = x.User.UserName,
                                    UserAvatar = x.User.Picture,
                                    Content = x.Content,
                                    ParentId = x.ParentId,
                                    CreatedDate = x.CreatedDate,
                                    ModifiedDate = x.ModifiedDate,
                                    Published = x.Published,
                                },
                                ReplyComments = new()
                            }).ToList();
                if (!comments.Any())
                {
                    return response;
                }
                var listParentId = comments.Select(x => x.Comment.Id).ToList();
                var listReply = _commentVideoRepo.FindAll(x => listParentId.Contains(x.ParentId) && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId))).Include(x => x.User).ToList();

                foreach (var item in comments)
                {
                    var rep = listReply.Where(x => x.ParentId == item.Comment.Id).OrderByDescending(x => x.CreatedDate);
                    var totalquantity = rep.Count();
                    if (totalquantity == 0)
                        continue;
                    var childRequest = listChildRequest?.SingleOrDefault(x => x.ParentId == item.Comment.Id);
                    var childQuantity = (childRequest!=null ? childRequest.PageIndex : 1) * (childRequest!=null ? childRequest.PageSize : 5);
                    item.ReplyComments.ReplyComments = rep.Take(childQuantity).Select(x => new CommentVideoModel
                    {
                        Id = x.Id,
                        VideoId = x.VideoId,
                        UserId = x.UserId,
                        Username = x.User.UserName,
                        UserAvatar = x.User.Picture,
                        Content = x.Content,
                        ParentId = x.ParentId,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate,
                        Published = x.Published,
                    }).ToList();
                    item.ReplyComments.ParentCommentId = item.Comment.Id;
                    item.ReplyComments.TotalQuantityRep = totalquantity;
                    item.ReplyComments.CurentQuantityRep = 5;
                }

                result.ListComment = comments;
                result.TotalQuantity = totalQuantity;
                result.CurentQuantity = pageIndex * pageSize;
            }
            response.Data = result;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public ResponseBase GetCommentReplyVideo(int userId, int parentCommentId, int pageIndex, int pageSize)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var result = new CommentReplyVideoModel()
            {
                ParentCommentId = parentCommentId,
                TotalQuantityRep = 0,
                CurentQuantityRep = 0,
            };

            var quantity = _commentVideoRepo.FindAll(x => x.ParentId == parentCommentId && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId))).Count();
            if (quantity == 0)
            {
                response.Data = result;
                return response;
            }

            var listReply = _commentVideoRepo.FindAll(x => x.ParentId == parentCommentId && x.Deleted == false
                                                        && ((x.UserId != userId && x.Published == true) || (x.UserId == userId)))
                                                .Include(x => x.User)
                                                .OrderByDescending(x => x.CreatedDate).Take(pageIndex * pageSize)
                                                .ToList();
            var data = listReply.Select(x => new CommentVideoModel
            {
                Id = x.Id,
                VideoId = x.VideoId,
                UserId = x.UserId,
                Username = x.User.UserName,
                UserAvatar = x.User.Picture,
                Content = x.Content,
                ParentId = x.ParentId,
                CreatedDate = x.CreatedDate,
                ModifiedDate = x.ModifiedDate,
                Published = x.Published,
            }).ToList();
            result.TotalQuantityRep = quantity;
            result.CurentQuantityRep = pageIndex * pageSize;
            result.ReplyComments = data;
            response.Data = result;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            return response;
        }
    }
    public async Task<ResponseBase> GetCommentVideoForApproval(string? videoTitle, bool? publish, int? categoryId,int pageIndex, int pageSize)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var data = new CommentVideoForApproval()
            {
                ListData = new List<CommentVideoData>(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Quantity = 0,
            };
            var listVideo = _videoRepository.FindAll(x => (string.IsNullOrEmpty(videoTitle) ? true : x.KeySearch.Contains(videoTitle))
                                                      && (categoryId.HasValue ? x.IdCategory == categoryId : true))
                                                        .Select(x => new
                                                        {
                                                            Id = x.Id,
                                                            Title = x.Title,
                                                        }).ToList();

            var listVideoIds = listVideo.Select(x => x.Id).ToList();

            var quantityComment = await _commentVideoRepo.CountWhere(x => listVideoIds.Contains(x.VideoId) && (publish.HasValue ? x.Published == publish : true) && x.Deleted == false);

            if ((pageIndex - 1) * pageSize > quantityComment)
            {
                response.Data = data;
                return response;
            }
            data.Quantity = quantityComment;
            var comments = _commentVideoRepo.FindAll(x => listVideoIds.Contains(x.VideoId) && (publish.HasValue ? x.Published == publish : true) && x.Deleted == false)
                                            .OrderByDescending(x => x.ModifiedDate)
                                            .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                                            .Select(x => new CommentVideoDetail
                                            {
                                                Id = x.Id,
                                                UserId = x.UserId,
                                                VideoId = x.VideoId,
                                                Content = x.Content,
                                                ParentId = x.ParentId,
                                                Publish = x.Published,
                                                CreatedDate = x.CreatedDate,
                                                ModifiedDate = x.ModifiedDate,
                                            }).ToList();
            data.ListData = comments.GroupBy(x => x.VideoId).Select(x => new CommentVideoData
            {
                VideoId = x.Key,
                VideoTitle = listVideo.FirstOrDefault(y => y.Id == x.Key).Title,
                ListComment = x.ToList()
            }).ToList();
            response.Code = 200;
            response.Data = data;
            return response;
        }
        catch (Exception ex)
        {
            response.Code = 400;
            response.Message = ex.Message;
            return response;
        }
    }

}

