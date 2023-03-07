using Dapper;
using GoStay.Common.Constants;
using GoStay.DataDto.Statistical;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
    public class StatisticalRepository
    {
        public static List<PriceByUserDto> GetAllOrderPriceByUser(int userID)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", userID, System.Data.DbType.Int32);

            return DapperExtensions.QueryDapperStoreProc<PriceByUserDto>(Procedures.sq_GetAllOrderPriceByUser, p).ToList();
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
        public static List<OrderByUserDto> GetAllOrderByUser(int userID, int pageIndex, int pageSize)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", userID, System.Data.DbType.Int32);
            p.Add("@PageIndex", pageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", pageSize, System.Data.DbType.Int32);

            return DapperExtensions.QueryDapperStoreProc<OrderByUserDto>(Procedures.sq_GetAllOrderByUser, p).ToList();
        }
    }
}
