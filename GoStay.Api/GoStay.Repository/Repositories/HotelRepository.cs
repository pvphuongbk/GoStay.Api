using Dapper;
using GoStay.Common;
using GoStay.Common.Constants;
using GoStay.Data.HotelDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
	public class HotelRepository
	{
		public static List<HotelHomePageDto> GetListHotelForHomePage(HotelSearchingRequest filter)
		{
			var p = new DynamicParameters();

			p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",",filter.IdPhuong), System.Data.DbType.String);
			p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
			p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
			p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
			p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
			p.Add("@ActualPrice", filter.Price == null ? null : filter.Price.ToString(), System.Data.DbType.String);
			p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);

			return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotel, p).ToList();
		}
        public static List<HotelHomePageDto> GetPagingListHotelForHomePage(HotelSearchingPaging filter)
        {
            var p = new DynamicParameters();

            p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",", filter.IdPhuong), System.Data.DbType.String);
            p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
            p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
            p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
            p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
            p.Add("@ActualPrice", filter.Price == null ? null : filter.Price.ToString(), System.Data.DbType.String);
            p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotelPaging, p).ToList();
        }

        public static List<HotelHomePageDto> GetListTopHotelForHomePage(int number)
        {
            var p = new DynamicParameters();
            var filter = new HotelSearchingPaging();
            filter.PageIndex = 1;
            filter.PageSize = number;
            p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",", filter.IdPhuong), System.Data.DbType.String);
            p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
            p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
            p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
            p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
            p.Add("@ActualPrice", filter.Price == null ? null : filter.Price.ToString(), System.Data.DbType.String);
            p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotelPaging, p).ToList();
        }
    }
}
