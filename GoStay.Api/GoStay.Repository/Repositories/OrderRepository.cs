using Dapper;
using GoStay.Common.Constants;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
    public class OrderRepository
    {
        public static List<OrderSearchOutDto> SearchListOrder(OrderSearchParam filter)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", filter.UserId == null ? null : string.Join(",", filter.UserId), System.Data.DbType.String);
            p.Add("@Email", filter.Email == null ? null : string.Join(",", filter.Email), System.Data.DbType.String);
            p.Add("@Phone", filter.Phone == null ? null : string.Join(",", filter.Phone), System.Data.DbType.String);
            p.Add("@OrderCode", filter.OrderCode == null ? null : string.Join(",", filter.OrderCode), System.Data.DbType.String);
            p.Add("@Status", filter.Status == null ? null : string.Join(",", filter.Status), System.Data.DbType.String);
            p.Add("@Style", filter.Style == null ? null : string.Join(",", filter.Style), System.Data.DbType.String);
            p.Add("@StartDate", filter.StartDate == null ? null : string.Join(",", filter.StartDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);
            p.Add("@EndDate", filter.EndDate == null ? null : string.Join(",", filter.EndDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);
            return DapperExtensions.QueryDapperStoreProc<OrderSearchOutDto>(Procedures.sq_SearchListOrder, p).ToList();
        }
    }
}
