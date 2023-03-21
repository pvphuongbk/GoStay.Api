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
            p.Add("@TextSearch", filter.TextSearch == null ? null : string.Join(",", filter.TextSearch), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);

            return DapperExtensions.QueryDapperStoreProc<NewSearchOutDto>(Procedures.sq_GetListForSearchNews, p).ToList();
        }
    }
}
