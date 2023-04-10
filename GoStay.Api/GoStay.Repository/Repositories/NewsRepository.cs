using Dapper;
using GoStay.Common.Constants;
using GoStay.DataDto.News;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
    public class NewsRepository
    {
        public static List<NewSearchOutDto> SearchListNews(GetListNewsParam filter)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", filter.UserId == null ? null : string.Join(",", filter.UserId), System.Data.DbType.String);
            p.Add("@Status", filter.Status == null ? null : string.Join(",", filter.Status), System.Data.DbType.String);
            p.Add("@IdCategory", filter.IdCategory == null ? null : string.Join(",", filter.IdCategory), System.Data.DbType.String);
            p.Add("@IdTopic", filter.IdTopic == null ? null : string.Join(",", filter.IdTopic), System.Data.DbType.String);
            p.Add("@TextSearch", filter.TextSearch == null ? null : string.Join(",", filter.TextSearch), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);

            return DapperExtensions.QueryDapperStoreProc<NewSearchOutDto>(Procedures.sq_GetListForSearchNews, p).ToList();
        }
        public static List<NewSearchOutDto> GetListTopNews(int IdCategory)
        {
            var p = new DynamicParameters();
            p.Add("@IdCategory", IdCategory == null ? null : string.Join(",", IdCategory), System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<NewSearchOutDto>(Procedures.sq_GetListTopNews, p).ToList();
        }
    }
}
