using Dapper;
using GoStay.Common.Configuration;
using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.RatingDto;
using System.Data;
using System.Reflection;


namespace GoStay.Repository.DapperHelper
{
    public class HotelDapperExtensions : DapperExtensions
    {
        public static HotelDetailSummaryDto GetHotelDetailDto(string store, int id, int userId)
        {
            var p = new DynamicParameters();
            p.Add("@IdHotel", id, System.Data.DbType.Int32);
            p.Add("@IdUser", userId, System.Data.DbType.Int32);

            using (IDbConnection conn = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                var summary = new HotelDetailSummaryDto();
                var reader = conn.QueryMultiple(store, p, commandType: CommandType.StoredProcedure);
                var hotelDetail = reader.Read<HotelDetailDto>().FirstOrDefault();
                hotelDetail.Pictures = reader.Read<string>().ToList();
                hotelDetail.Rooms = reader.Read<HotelRoomDto>().ToList();
                hotelDetail.Services = reader.Read<ServiceDetailHotelDto>().ToList();
                var roomServices = reader.Read<ServiceDetailHotelDto>().ToList();

                foreach(var rm in hotelDetail.Rooms)
                {
                    rm.Services = roomServices.Where(x => x.IdRoom == rm.Id).ToList();
                    rm.Pictures = rm.StrPictures.Split(';').ToList();
                }

                hotelDetail.Review_score = (hotelDetail.ReviewScore == null || hotelDetail.NumberReviewers == null) ? -1 :
                                        (double)(hotelDetail.ReviewScore / hotelDetail.NumberReviewers);

                var room = hotelDetail.Rooms.Where(x => x.Discount != null).MinBy(x => x.NewPrice);
                if (room != null)
                {
                    hotelDetail.Discount = room.Discount;
                    hotelDetail.OriginalPrice = (decimal)room.PriceValue;
                    hotelDetail.ActualPrice = (decimal)room.NewPrice;
                }

                hotelDetail.Slug = hotelDetail.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower();

                summary.UserBoxReviews = reader.Read<UserBoxReview>().ToList();
                summary.HotelDetailDto = hotelDetail;
                return summary;
            }
        }
    }
}
