using Dapper;
using GoStay.Common.Constants;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Statistical;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
    public class StatisticalRepository
    {
        public static List<PriceDetailByUserDto> GetAllOrderPriceByUser(PriceDetailByUserRequest request)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", request.UserID, System.Data.DbType.Int32);
            p.Add("@StartDate", request.StartDate, System.Data.DbType.DateTime);
            p.Add("@EndDate", request.EndDate, System.Data.DbType.DateTime);

            return DapperExtensions.QueryDapperStoreProc<PriceDetailByUserDto>(Procedures.sq_GetAllOrderPriceByUser, p).ToList();
        }

        public static List<PriceChartByUserDto> GetPriceChartByUserInYear(int userID, int year)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", userID, System.Data.DbType.Int32);
            p.Add("@Year", year, System.Data.DbType.Int16);

            return DapperExtensions.QueryDapperStoreProc<PriceChartByUserDto>(Procedures.sq_GetPriceChartByUserInYear, p).ToList();
        }
        public static List<PriceChartByUserDto> GetPriceChartByUserInMonth(int userID, int year, int month)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", userID, System.Data.DbType.Int32);
            p.Add("@Year", year, System.Data.DbType.Int16);
            p.Add("@Month", month, System.Data.DbType.Int16);

            return DapperExtensions.QueryDapperStoreProc<PriceChartByUserDto>(Procedures.sq_GetPriceChartByUserInMonth, p).ToList();
        }
        public static List<OrderByUserDto> GetAllOrderByUser(int userID, int pageIndex, int pageSize, int style)
        {
            
            var p = new DynamicParameters();
            p.Add("@UserId", userID, System.Data.DbType.Int32);
            p.Add("@PageIndex", pageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", pageSize, System.Data.DbType.Int32);

            if (style == 2)
                return DapperExtensions.QueryDapperStoreProc<OrderByUserDto>(Procedures.sq_GetAllOrderTourByUser, p).ToList();

            return DapperExtensions.QueryDapperStoreProc<OrderByUserDto>(Procedures.sq_GetAllOrderRoomByUser, p).ToList();

        }
    }
}
