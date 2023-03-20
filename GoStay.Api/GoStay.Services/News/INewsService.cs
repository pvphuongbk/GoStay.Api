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

    }
}
