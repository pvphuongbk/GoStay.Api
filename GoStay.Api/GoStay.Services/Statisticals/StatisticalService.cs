using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Utilities;
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
                chart.TotalSizeImg = (long)_pictureRepository.FindAll(x => x.Deleted != 1).Select(x => x.Size).Sum() / 1000000;
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
    }
}
