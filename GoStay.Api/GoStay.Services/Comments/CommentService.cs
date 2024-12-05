﻿using AutoMapper;
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
    public ResponseBase GetCommentNews(int userId,int newsId,int pageIndex, int pageSize)
    {
        ResponseBase response = new ResponseBase();
        try
        {
            var result =new CommentNewsResponseModel();
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                result.UserName = "";
                result.UserAvatar = "";
            }
            else
            {
                result.UserName = user.FirstName + user.LastName;
                result.UserAvatar = user.Picture;
            }
            result.UserId = userId;
            var comments = new List<CommentNewsViewModel>();
            var news = _newsRepository.GetById(newsId);
            if (news==null)
            {
                response.Message = "News not existing";
                return response;
            }
            comments = _commentNewsRepo.FindAll(x => x.NewsId == newsId && x.ParentId == 0)
                        .Include(x => x.User).Take(pageIndex * pageSize).Select(x => new CommentNewsViewModel
                        {
                            Comment = new CommentNewsModel
                            {
                                Id = x.Id,
                                NewsId = x.NewsId,
                                UserId = x.UserId,
                                Username = x.User.FirstName + x.User.LastName,
                                UserAvatar = x.User.Picture,
                                Content = x.Content,
                                ParentId = x.ParentId,
                                CreatedDate = x.CreatedDate,
                                ModifiedDate = x.ModifiedDate,
                            },
                            ReplyComments = new()
                        }).ToList();
            if(!comments.Any())
            {
                return response;
            }
            var listParentId = comments.Select(x=>x.Comment.Id).ToList();
            var listReply = _commentNewsRepo.FindAll(x => listParentId.Contains(x.ParentId)).Include(x => x.User).ToList();

            foreach (var item in comments)
            {
                var rep = listReply.Where(x => x.ParentId == item.Comment.Id);
                if (!listReply.Any())
                    continue;
                item.ReplyComments = rep.Select(x=> new CommentNewsModel
                {
                    Id = x.Id,
                    NewsId = x.NewsId,
                    UserId = x.UserId,
                    Username = x.User.FirstName + x.User.LastName,
                    UserAvatar = x.User.Picture,
                    Content = x.Content,
                    ParentId = x.ParentId,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate,
                    
                }).ToList();
            }
            result.ListComment = comments;
            response.Data = result;
            return response;
        }
        catch (Exception ex) 
        { 
            response.Message = ex.Message;
            return response;
        }
    }
}

