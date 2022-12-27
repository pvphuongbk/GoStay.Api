using Dapper;
using GoStay.Common.Constants;
using GoStay.Common.Extention;
using GoStay.Data.HotelDto;
using GoStay.Data.TourDto;
using GoStay.Repository.DapperHelper;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace GoStay.Repository.Repositories
{
    public class TourRepository
    {
        public static List<SearchTourDto> GetPagingListTours(SearchTourRequest filter)
        {
            var p = new DynamicParameters();
            p.Add("@IdTourTopic", filter.IdTourTopic == null ? null : string.Join(",", filter.IdTourTopic), System.Data.DbType.String);
            p.Add("@IdTourStyle", filter.IdTourStyle == null ? null : string.Join(",", filter.IdTourStyle), System.Data.DbType.String);
            p.Add("@IdProvinceFrom", filter.IdProvinceFrom == null ? null : string.Join(",", filter.IdProvinceFrom), System.Data.DbType.String);
            p.Add("@IdProvinceTo", filter.IdProvinceTo == null ? null : string.Join(",", filter.IdProvinceTo), System.Data.DbType.String);
            p.Add("@ActualPrice", filter.ActualPrice == null ? null  : filter.ActualPrice.ToString(), System.Data.DbType.String);
            p.Add("@Rating", filter.Rating == null ? null : string.Join(",", filter.Rating), System.Data.DbType.String);
            p.Add("@StartDate", filter.StartDate == null ? null : string.Join(",", filter.StartDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);
            p.Add("@EndDate", filter.EndDate == null ? null : string.Join(",", filter.EndDate?.ToString("yyyy-MM-dd")), System.Data.DbType.String);
            p.Add("@PageIndex", filter.PageIndex, System.Data.DbType.Int32);
            p.Add("@PageSize", filter.PageSize, System.Data.DbType.Int32);


            return DapperExtensions.QueryDapperStoreProc<SearchTourDto>(Procedures.sq_GetListForSearchTours, p).ToList();
        }


        public static List<SuggestTourDto> SuggestTour(string searchText)
        {
            var p = new DynamicParameters();
            p.Add("@SearchText", searchText, System.Data.DbType.String);

            return DapperExtensions.QueryDapperStoreProc<SuggestTourDto>(Procedures.sq_SuggestSearchTour, p).ToList();
        }
    }
}
