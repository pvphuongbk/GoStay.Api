using Dapper;
using GoStay.Common;
using GoStay.Common.Constants;
using GoStay.Data.HotelDto;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.RatingDto;
using GoStay.Repository.DapperHelper;

namespace GoStay.Repository.Repositories
{
	public class HotelRepository
	{

        public static List<HotelHomePageDto> GetPagingListHotelForHomePage(HotelSearchRequest filter)
        {
            var p = new DynamicParameters();
            p.Add("@Palletbed", filter.Palletbed == null ? null : string.Join(",", filter.Palletbed), System.Data.DbType.String);
            p.Add("@NumMature", filter.NumMature == null ? null : string.Join(",", filter.NumMature), System.Data.DbType.String);
            p.Add("@NumChild", filter.NumChild == null ? null : string.Join(",", filter.NumChild), System.Data.DbType.String);
            p.Add("@NumRoom", filter.NumRoom == null ? null : string.Join(",", filter.NumRoom), System.Data.DbType.String);
            p.Add("@CheckinDate", filter.CheckinDate == null ? null : string.Join(",", filter.CheckinDate?.ToString("yyyy-MM-dd HH:mm:ss")), System.Data.DbType.String);
            p.Add("@CheckoutDate", filter.CheckoutDate == null ? null : string.Join(",", filter.CheckoutDate?.ToString("yyyy-MM-dd HH:mm:ss")), System.Data.DbType.String);
            p.Add("@IdTinhThanh", filter.IdTinhThanh == null ? null : string.Join(",", filter.IdTinhThanh), System.Data.DbType.String);
            p.Add("@IdPhuongs", filter.IdPhuong == null ? null : string.Join(",", filter.IdPhuong), System.Data.DbType.String);
            p.Add("@IdQuans", filter.IdQuans == null ? null : string.Join(",", filter.IdQuans), System.Data.DbType.String);
            p.Add("@Criterion", filter.Criterion == null ? null : string.Join(",", filter.Criterion), System.Data.DbType.String);

            p.Add("@Rating", filter.Ratings == null ? null : string.Join(",", filter.Ratings), System.Data.DbType.String);
            p.Add("@Types", filter.Types == null ? null : string.Join(",", filter.Types), System.Data.DbType.String);
            p.Add("@IdServicesHotel", filter.ServicesHotel == null ? null : string.Join(",", filter.ServicesHotel), System.Data.DbType.String);
            p.Add("@IdServicesRoom", filter.ServicesRoom == null ? null : string.Join(",", filter.ServicesRoom), System.Data.DbType.String);

            p.Add("@ActualPriceMin", filter.PriceMin == null ? null : filter.PriceMin.ToString(), System.Data.DbType.String);
            p.Add("@ActualPriceMax", filter.PriceMax == null ? null : filter.PriceMax.ToString(), System.Data.DbType.String);
            p.Add("@ReviewScore", filter.ReviewScore == null ? null : filter.ReviewScore.ToString(), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListForSearchHotelPaging_gamma, p).ToList();
        }
        public static List<HotelHomePageDto> GetListHotelHomepage(HotelSearchRequest filter)
        {
            var p = new DynamicParameters();

            p.Add("@IdTinhThanh", filter.IdTinhThanh == null ? null : string.Join(",", filter.IdTinhThanh), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<HotelHomePageDto>(Procedures.sq_GetListHotelHomepage, p).ToList();
        }
        public static List<LocationDropdownDto> GetListLocationForDropdown(string searchText)
        {
            var p = new DynamicParameters();
            p.Add("@SearchText", searchText, System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<LocationDropdownDto>(Procedures.sq_GetListLocationForDropdown, p).ToList();
        }

        public static List<LocationDropdownDto> GetListNearHotel(int NumTop,float LAT, float LON)
        {
            var p = new DynamicParameters();
            p.Add("@LAT", LAT, System.Data.DbType.Double);
            p.Add("@NumTop", NumTop, System.Data.DbType.Int32);

            p.Add("@LON", LON, System.Data.DbType.Double);

            return DapperExtensions.QueryDapperStoreProc<LocationDropdownDto>(Procedures.sq_GetListNearHotel, p).ToList();
        }

        public static List<RoomAdminDto> GetListRoomAdmin(RequestGetListRoomAdmin filter)
        {
            var p = new DynamicParameters();
            p.Add("@IdHotel", filter.IdHotel == null ? null : string.Join(",", filter.IdHotel), System.Data.DbType.String);
            p.Add("@IdUser", filter.IdUser == null ? null : string.Join(",", filter.IdUser), System.Data.DbType.String);
            p.Add("@RoomStatus", filter.RoomStatus == null ? null : string.Join(",", filter.RoomStatus), System.Data.DbType.String);
            p.Add("@HotelName", filter.HotelName == null ? null : string.Join(",", filter.HotelName), System.Data.DbType.String);
            p.Add("@RoomName", filter.RoomName == null ? null : string.Join(",", filter.RoomName), System.Data.DbType.String);


            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<RoomAdminDto>(Procedures.sq_GetListRoomAdmin, p).ToList();
        }

        public static List<RatingAdminDto> GetListRating(int? HotelId, byte? Status,string? NameSearch, int PageIndex, int PageSize)
        {
            var p = new DynamicParameters();
            p.Add("@HotelId", HotelId == null ? null : string.Join(",", HotelId), System.Data.DbType.String);
            p.Add("@Status", Status == null ? null : string.Join(",", Status), System.Data.DbType.String);
            p.Add("@NameSearch", NameSearch, System.Data.DbType.String);
            p.Add("@PageIndex", PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<RatingAdminDto>(Procedures.sq_GetListRating, p).ToList();
        }
    }
}
