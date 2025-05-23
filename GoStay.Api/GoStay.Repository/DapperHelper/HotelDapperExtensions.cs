﻿using Dapper;
using GoStay.Common.Configuration;
using GoStay.Common.Constants;
using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.RatingDto;
using GoStay.DataDto.Scheduler;
using Microsoft.Data.SqlClient.Server;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using SqlData = System.Data.SqlClient;

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

                foreach (var rm in hotelDetail.Rooms)
                {
                    rm.Services = roomServices.Where(x => x.IdRoom == rm.Id).OrderBy(x=>x.AdvantageLevel).Take(5).ToList();
                    rm.Pictures = string.IsNullOrEmpty(rm.StrPictures) ? new List<string>() : rm.StrPictures.Split(';').ToList();
                }

                hotelDetail.Review_score = (hotelDetail.ReviewScore == null || hotelDetail.NumberReviewers == null) ? -1 :
                                        (double)(hotelDetail.ReviewScore / hotelDetail.NumberReviewers);

                var room = hotelDetail.Rooms.Where(x => x.Discount != null).MinBy(x => x.NewPrice);
                if (room != null)
                {
                    hotelDetail.Discount = room.Discount;
                    hotelDetail.OriginalPrice = (decimal)room.CurrentPrice;
                    hotelDetail.ActualPrice = (decimal)room.NewPrice;
                }

                hotelDetail.Slug = hotelDetail.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower();

                summary.UserBoxReviews = reader.Read<UserBoxReview>().ToList();
                summary.HotelDetailDto = hotelDetail;
                return summary;
            }
        }


        public static bool ScheduleRoomPrice(List<SchedulerRoomPriceDto> items)
        {
            try
            {
                using (SqlData.SqlConnection con = new SqlData.SqlConnection(ConnectionString))
                {
                    DataTable dt = new DataTable();
                    DataColumn id = new DataColumn("RoomId");
                    DataColumn pri = new DataColumn("Price");
                    dt.Columns.Add(id);
                    dt.Columns.Add(pri);
                    items.ToList().ForEach(dr => { DataRow d = dt.NewRow(); d[id] = dr.RoomId; d[pri] = dr.Price; dt.Rows.Add(d); });


                    SqlCommand cmd = new SqlCommand($"exec {Procedures.sq_schedule_Update_Room_Price} @PriceByRoom", con);
                    var pXeOnlines = new SqlParameter("@PriceByRoom", SqlDbType.Structured);
                    pXeOnlines.TypeName = "dbo.PriceByRoom";
                    pXeOnlines.Value = dt;
                    cmd.Parameters.Add(pXeOnlines);

                    con.Open();
                    var sReader = cmd.ExecuteReader();

                    while (sReader.Read())
                    {
                        var re = sReader["Result"] == null ? 0 : int.Parse(sReader["Result"].ToString());
                        return re > 0;
                    }
                    con.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                //FileHelper.GeneratorFileByDay(FileStype.Error, ex.ToString(), "CarBll." + MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        private static List<SqlDataRecord> CreatePriceByRoomType(List<SchedulerRoomPriceDto> items)
        {
            try
            {
                List<SqlDataRecord> datatable = new List<SqlDataRecord>();
                SqlMetaData[] sqlMetaData = new SqlMetaData[2];
                sqlMetaData[0] = new SqlMetaData("RoomId", SqlDbType.Int);
                sqlMetaData[1] = new SqlMetaData("Price", SqlDbType.Decimal);

                foreach (var item in items)
                {
                    var row = new SqlDataRecord(sqlMetaData);
                    row.SetValues(new object[]
                    {
                        item.RoomId,
                        item.Price,
                    });

                    datatable.Add(row);
                }

                return datatable;
            }
            catch (Exception ex)
            {
                //FileHelper.GeneratorFileByDay(FileStype.Error, ex.ToString(), "CarBll." + MethodBase.GetCurrentMethod().Name);
                return new List<SqlDataRecord>();
            }
        }
    }
}
