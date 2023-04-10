using GoStay.Data.Base;
using GoStay.DataDto.News;

namespace GoStay.Services.Newss
{
    public interface INewsService
    {
        public ResponseBase AddNews(NewsDto news);
        public ResponseBase EditNews(NewsDto news);
        public ResponseBase DeleteNews(int Id);
        public ResponseBase GetListNews(GetListNewsParam param);
        public ResponseBase GetNews(int Id);
        public ResponseBase EditContentNews(string content, int NewsId);
        public ResponseBase EditPictureTitleNews(string url, int NewsId);
        public ResponseBase GetListNewsHomePage();
        public ResponseBase GetListTopNewsByCategory(int? IdCategory,int? IdTopic);

    }
}
