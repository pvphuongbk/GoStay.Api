﻿using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.News;
using GoStay.DataDto.Video;

namespace GoStay.Services.Newss
{
    public interface INewsService
    {
        public ResponseBase GetNewsDefault(int idUser, int idNews);
        public ResponseBase GetListNews2(GetListNewsParam param);
        public ResponseBase SubmitNews(NewsDataDto news);
        public ResponseBase UpdateStatusNews(UpdateStatusNewsParam param);
        ResponseBase GetNewsForHomePage(int latestQuantity, int categoryQuantity, int hotQuantlty, DateTime dateStart, DateTime dateEnd, int idcategory, int idtopic);

        public ResponseBase AddNews(NewsDto news);
        public ResponseBase EditNews(NewsDto news);
        public ResponseBase DeleteNews(int Id);
        public ResponseBase GetListNews(GetListNewsParam param);
        public ResponseBase GetNews(int Id, int? domain = null);
        public ResponseBase EditContentNews(string content, int NewsId);
        public ResponseBase TopicDetailNew(int max);
        public ResponseBase TopicDetailNew(int max, int domain);
        public ResponseBase EditPictureTitleNews(string url, int NewsId);
        public ResponseBase GetListNewsHomePage();
        public ResponseBase GetListTopNewsByCategory(int? IdCategory, int? IdTopic);
        public ResponseBase EditClickNews(int NewsId);
        public ResponseBase GetListVideoNews(GetListVideoNewsParam filter);
        public ResponseBase AddVideoNews(VideoModel news);
        public ResponseBase EditVideoNews(EditVideoNewsDto news);
        public ResponseBase DeleteVideoNews(int Id);
        public ResponseBase GetVideoNews(int Id);
        public ResponseBase GetDataSupportNews(int IdDoMain);
        public ResponseBase GetNewsTopicTotal(int IdDomain);
        public ResponseBase GetNewsCategoryTotal(int IdDomain);
        public ResponseBase GetListCategoryByParentId(int IdDomain, int ParentId);

        public ResponseBase GetNearHotel(int videoId);
        public ResponseBase UpsertVideo(CreateVideoModel video);
        public ResponseBase GetDefaultVideo(int idUser);
        public ResponseBase GetVideoById(int id);
        public ResponseBase GetTagNews();
        public ResponseBase GetCategoryNews();
        public ResponseBase GetNewsByTopicAndCategory(int idCategory, int idTopic, int pageIndex, int pageSize);
        public ResponseBase<List<NewsHomeData>> GetNewsByKeyword(string keyword, int pageIndex, int pageSize);
    }
}
