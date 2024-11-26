using GoStay.Common.Enums;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Utilities;
using GoStay.DataDto.Statistical;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoStay.Services.Statisticals
{
    public class StatisticalService : IStatisticalService
    {
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
        private readonly ICommonRepository<PriceRange> _priceRangeRepository;
        private readonly ICommonRepository<TypeHotel> _typeHotelRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;

        public StatisticalService(ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelRoom> hotelRoomRepository, 
            ICommonRepository<PriceRange> priceRangeRepository, ICommonRepository<TypeHotel> typeHotelRepository, ICommonRepository<Picture> pictureRepository)
        {
            _hotelRepository = hotelRepository;
            _hotelRoomRepository = hotelRoomRepository;
            _priceRangeRepository = priceRangeRepository;
            _typeHotelRepository = typeHotelRepository;
            _pictureRepository = pictureRepository;
        }
        public ResponseBase GetValueChart()
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                ChartDto chart = new ChartDto();
                chart.TotalSizeImg = 0;
                var hotels = _hotelRepository.FindAll(x => x.Deleted != 1)
                                    .Select(x => new HotelRatingForChartDto { Rating = x.Rating }).ToList();
                var types = _typeHotelRepository.FindAll(x => x.Deleted != 1)
                                                    .Include(x => x.Hotels)
                                                    .Select(x => new HotelTypeForChartDto { Type = x.Type, Count = x.Hotels.Count(c => c.Deleted != 1) } )
                                                    .ToList();
                
                var priceRanges = _priceRangeRepository.FindAll(x => x.Deleted != 1)
                                                           .Include(x => x.Hotels.Where(c => c.Deleted != 1))
                                                           .Select(x => new PriceRangeForChartDto { Title = x.TitleVnd, Count = x.Hotels.Count(c => c.Deleted != 1) })
                                                           .ToList();
                var hotelRooms = _hotelRoomRepository.FindAll(x => x.Deleted != 1)
                                                        .Select(x => new CreateRoomForChartDto { CreatedDate = x.CreatedDate }).ToList();
                chart.TotalHotel = hotels.Count;
                chart.TotalRoom = hotelRooms.Count;
                chart.TotalImg = _pictureRepository.FindAll(x => x.Deleted != 1).Count();
                chart.TotalSizeImg = (long)_pictureRepository.FindAll(x => x.Deleted != 1).Select(x => x.Size/100).Sum() / 10000;
                for (int i = 0; i <= 5; i++)
                {
                    var count = hotels.Count(x => x.Rating == i);
                    chart.HotelRating.Add(i.ToString() + " sao", new ChartValue
                    {
                        Count = count,
                        Percent = Math.Round(((double)count * 100 / chart.TotalHotel), 2)
                    });
                }
                foreach (var type in types)
                {
                    if (string.IsNullOrEmpty(type.Type) || chart.TypeHotel.ContainsKey(type.Type))
                        continue;
                    chart.TypeHotel.Add(type.Type, new ChartValue
                    {
                        Count = type.Count,
                        Percent = Math.Round(((double)type.Count * 100 / chart.TotalHotel), 2)
                    });
                }
                foreach (var priceRange in priceRanges)
                {
                    if (string.IsNullOrEmpty(priceRange.Title) || chart.TypeHotel.ContainsKey(priceRange.Title))
                        continue;
                    chart.PriceRange.Add(priceRange.Title, new ChartValue
                    {
                        Count = priceRange.Count,
                        Percent = Math.Round(((double)priceRange.Count * 100 / chart.TotalHotel), 2)
                    });
                }

                var maxDate = DateTime.Now;//hotelRooms.Max(x => x.CreatedDate);
                var minDate = hotelRooms.Min(x => x.CreatedDate);
                for (DateTime i = minDate; i <= maxDate;i = i.AddMonths(1))
                {
                    var key = i.ToString("MM-yyyy");
                    if (string.IsNullOrEmpty(key) || chart.RoomByMonth.ContainsKey(key))
                        continue;

                    var count = hotelRooms.Count(x => (x.CreatedDate.Year == i.Year && x.CreatedDate.Month <= i.Month) 
                                || x.CreatedDate.Year < i.Year);
                    chart.RoomByMonth.Add(key, new ChartValue
                    {
                        Count = count,
                        //Percent = Math.Round(((double)count * 100 / chart.TotalRoom), 2)
                    });
                }

                chart.HotelRating = chart.HotelRating.OrderByDescending(x => x.Value.Percent).ToDictionary(x => x.Key, x => x.Value);
                chart.PriceRange = chart.PriceRange.OrderByDescending(x => x.Value.Percent).ToDictionary(x => x.Key, x => x.Value);
                chart.TypeHotel = chart.TypeHotel.OrderByDescending(x => x.Value.Percent).ToDictionary(x => x.Key, x => x.Value);
                chart.RoomByMonth = chart.RoomByMonth.OrderByDescending(x => x.Value.Percent).ToDictionary(x => x.Key, x => x.Value);

                responseBase.Data = chart;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase GetRoomInMonthByDay(int month, int year)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                RoomByDayDto chart = new RoomByDayDto();

                var hotelRooms = _hotelRoomRepository.FindAll(x => x.Deleted != 1 && x.CreatedDate.Year == year && x.CreatedDate.Month == month)
                                            .Select(x => new CreateRoomForChartDto { CreatedDate = x.CreatedDate }).ToList();
                chart.TotalRoom = hotelRooms.Count;
                var minDate = new DateTime(year, month, 1);
                var endDate = minDate.AddMonths(1);
                var maxDate = endDate < DateTime.Now.Date ? endDate : DateTime.Now.Date.AddDays(1);
                for (DateTime i = minDate; i < maxDate; i = i.AddDays(1))
                {
                    var count = hotelRooms.Count(x => x.CreatedDate.Day == i.Day);
                    chart.RoomByDayValue.Add(new RoomByDayValue
                    {
                        Amount = count,
                        Day = i.Day
                    });
                }
                responseBase.Data = chart;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public Task<ResponseBase> GetAllOrderPriceByUser(PriceDetailByUserRequest request)
        {
            return Task.Run(delegate
            {
                ResponseBase responseBase = new ResponseBase();
                try
                {
                    var data = StatisticalRepository.GetAllOrderPriceByUser(request);
                    responseBase.Data = data;
                    return responseBase;
                }
                catch (Exception e)
                {
                    responseBase.Code = ErrorCodeMessage.Exception.Key;
                    responseBase.Message = e.Message;
                    return responseBase;
                }
            });
        }

        public Task<ResponseBase> GetPriceChartByUser(PriceChartType type,int userID, int year,int month)
        {
            return Task.Run(delegate
            {
                ResponseBase responseBase = new ResponseBase();
                try
                {
                    Dictionary<int, Dictionary<int, decimal>> resultDic = new Dictionary<int, Dictionary<int, decimal>>();
                    if (type == PriceChartType.InYear)
                    {
                        var datas = StatisticalRepository.GetPriceChartByUserInYear(userID, year);
                        for(var status = 1; status <= 3; status ++)
                        {
                            Dictionary<int, decimal> dic = new Dictionary<int, decimal>();
                            for (DateTime date = new DateTime(year, 1, 1); date < new DateTime(year + 1, 1, 1); date = date.AddMonths(1))
                            {
                                var value = datas.FirstOrDefault(x => x.Keys.Month == date.Month && x.Status == status);
                                var price = value == null ? 0 : value.Value;
                                dic.Add(date.Month, price);
                            }

                            resultDic.Add(status, dic);
                        }
                    }
                    else if (type == PriceChartType.InMonth)
                    {
                        var datas = StatisticalRepository.GetPriceChartByUserInMonth(userID, year, month);
                        for (var status = 1; status <= 3; status++)
                        {
                            Dictionary<int, decimal> dic = new Dictionary<int, decimal>();
                            for (DateTime date = new DateTime(year, month, 1); date < new DateTime(year, month + 1, 1); date = date.AddDays(1))
                            {
                                var value = datas.FirstOrDefault(x => x.Keys.Day == date.Day && x.Status == status);
                                var price = value == null ? 0 : value.Value;
                                dic.Add(date.Day, price);
                            }

                            resultDic.Add(status, dic);
                        }
                    }

                    responseBase.Data = resultDic;
                    return responseBase;
                }
                catch (Exception e)
                {
                    responseBase.Code = ErrorCodeMessage.Exception.Key;
                    responseBase.Message = e.Message;
                    return responseBase;
                }
            });
        }

        public Task<ResponseBase> GetAllOrderByUser(int userID, int pageIndex, int pageSize, int style)
        {
            return Task.Run(delegate
            {
                ResponseBase responseBase = new ResponseBase();
                try
                {

                    var datas = StatisticalRepository.GetAllOrderByUser(userID, pageIndex, pageSize,style);
                    datas.ForEach(x => x.TotalPage = (x.TotalCount+ pageSize-1) / pageSize);
                    datas.ForEach(x=>x.Slug = x.HotelName?.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                            .Replace(".", "-")
                            .Replace("/", "-").Replace("--", string.Empty)
                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                            .Replace("(", string.Empty).Replace(")", string.Empty)
                            .Replace("*", string.Empty).Replace("%", string.Empty)
                            .Replace("&", "-").Replace("@", string.Empty).ToLower());
                    responseBase.Data = datas;
                    return responseBase;
                }
                catch (Exception e)
                {
                    responseBase.Code = ErrorCodeMessage.Exception.Key;
                    responseBase.Message = e.Message;
                    return responseBase;
                }
            });
        }
    }
}
