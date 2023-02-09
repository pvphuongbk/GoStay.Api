using Dapper;
using GoStay.Common.Constants;
using GoStay.Common.Extention;
using GoStay.Data.HotelDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.DapperHelper;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

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
            p.Add("@StartDate", filter.StartDate == null ? null : string.Join(",", filter.StartDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);
            p.Add("@EndDate", filter.EndDate == null ? null : string.Join(",", filter.EndDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<OrderSearchOutDto>(Procedures.sq_SearchListOrder, p).ToList();
        }

    }
}
