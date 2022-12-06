using Dapper;
using GoStay.Common;
using GoStay.Common.Constants;
using GoStay.Data.HotelDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
	public class HotelRepository
	{
		public static List<HotelHomePageDto> GetListHotelForHomePage(HotelSearchRequest filter)
		{
			var p = new DynamicParameters();

			p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",",filter.IdPhuong), System.Data.DbType.String);
			p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
			p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
			p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
			p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
			p.Add("@ActualPrice", filter.PriceMax == null ? null : filter.PriceMax.ToString(), System.Data.DbType.String);
			p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);

			return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotel, p).ToList();
		}
        public static List<HotelHomePageDto> GetPagingListHotelForHomePage(HotelSearchRequest filter)
        {
            var p = new DynamicParameters();
            p.Add("@IdTinhThanh", filter.IdTinhThanh == null ? null : string.Join(",", filter.IdTinhThanh), System.Data.DbType.String);
            p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",", filter.IdPhuong), System.Data.DbType.String);
            p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
            p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
            p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
            p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
            p.Add("@ActualPriceMin", filter.PriceMin == null ? null : filter.PriceMin.ToString(), System.Data.DbType.String);
            p.Add("@ActualPriceMax", filter.PriceMax == null ? null : filter.PriceMax.ToString(), System.Data.DbType.String);
            p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotelPaging_beta, p).ToList();
        }
        public static List<HotelHomePageDto> GetListHotelForHomePageNew(SeachHomePageDto search)
        {
            var p = new DynamicParameters();

            p.Add("@TypeAddress", search.TypeAddress.ToString(), System.Data.DbType.String);
            p.Add("@AddressId", search.AddressId.ToString(), System.Data.DbType.String);
            p.Add("@Palletbed", search.Palletbed.ToString(), System.Data.DbType.String);
            p.Add("@NumChild", search.NumChild.ToString(), System.Data.DbType.String);
            p.Add("@NumMature", search.NumMature.ToString(), System.Data.DbType.String);
            p.Add("@CheckinDate", search.CheckinDate.ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String);
            p.Add("@CheckoutDate", search.CheckoutDate.ToString("yyyy-MM-dd HH:mm:ss"), System.Data.DbType.String);
            p.Add("@PageIndex", search.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", search.PageSize, System.Data.DbType.Int32);

            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListHotelForHomePage, p).ToList();
        }
        
        public static List<HotelHomePageDto> GetListHotelTopFlashSale(int number)
        {
            var p = new DynamicParameters();
            var filter = new HotelSearchRequest();
            filter.PageIndex = 1;
            filter.PageSize = number;
            p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",", filter.IdPhuong), System.Data.DbType.String);
            p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
            p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
            p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
            p.Add("@IdServices", filter.Services == null ? null : string.Join(",", filter.Services), System.Data.DbType.String);
            p.Add("@ActualPrice", filter.PriceMax == null ? null : filter.PriceMax.ToString(), System.Data.DbType.String);
            p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotelPaging, p).ToList();
        }

        public static List<LocationDropdownDto> GetListLocationForDropdown(string searchText)
        {
            var p = new DynamicParameters();
            p.Add("@SearchText", searchText, System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<LocationDropdownDto>(Procedures.sq_GetListLocationForDropdown, p).ToList();
        }
    }
}
