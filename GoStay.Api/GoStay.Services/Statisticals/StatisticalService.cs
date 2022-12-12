using GoStay.Data.Base;
using GoStay.Data.Statistical;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GoStay.Services.Statisticals
{
    public class StatisticalService : IStatisticalService
    {
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
        private readonly ICommonRepository<PriceRange> _priceRangeRepository;
        private readonly ICommonRepository<TypeHotel> _typeHotelRepository;
        public StatisticalService(ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelRoom> hotelRoomRepository, ICommonRepository<PriceRange> priceRangeRepository, ICommonRepository<TypeHotel> typeHotelRepository)
        {
            _hotelRepository = hotelRepository;
            _hotelRoomRepository = hotelRoomRepository;
            _priceRangeRepository = priceRangeRepository;
            _typeHotelRepository = typeHotelRepository;
        }
        public ResponseBase GetValueChart()
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                ChartDto chart = new ChartDto();
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
                        Percent = Math.Round(((double)priceRange.Count * 100 / chart.TotalRoom), 2)
                    });
                }

                var maxDate = hotelRooms.Max(x => x.CreatedDate);
                var minDate = hotelRooms.Min(x => x.CreatedDate);
                for (DateTime i = minDate; i <= maxDate;i = i.AddMonths(1))
                {
                    var key = i.ToString("MM-yyyy");
                    if (string.IsNullOrEmpty(key) || chart.TypeHotel.ContainsKey(key))
                        continue;
                    var count = hotelRooms.Count(x => x.CreatedDate.Year == i.Year && x.CreatedDate.Month == i.Month);
                    chart.RoomByMonth.Add(key, new ChartValue
                    {
                        Count = count,
                        Percent = Math.Round(((double)count * 100 / chart.TotalRoom), 2)
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
