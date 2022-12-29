using AutoMapper;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helpers.Hotels
{
    public class HotelFunction : IHotelFunction
    {
        private readonly IMapper _mapper;

        public HotelFunction(IMapper mapper)
        {
            _mapper = mapper;
        }
        public HotelDetailDto CreateHotelDetailDto(Hotel hotel)
        {
            var hotelDto = new HotelDetailDto
            {
                Id = hotel.Id,
                HotelAddress = hotel.Address,
                HotelName = hotel.Name!,
                AvgNight = hotel.AvgNight,
                Lon_map = hotel.LonMap,
                Lat_map = hotel.LatMap,
                Rating = hotel.Rating,
                Review_score = (hotel.ReviewScore == null || hotel.NumberReviewers == null) ? -1 :
                                (double)(hotel.ReviewScore / hotel.NumberReviewers),
                ServiceScore = hotel.ServiceScore,
                ValueScore = hotel.ValueScore,
                SleepQualityScore = hotel.SleepQualityScore,
                CleanlinessScore = hotel.CleanlinessScore,
                LocationScore = hotel.LocationScore,
                RoomsScore = hotel.RoomsScore,
                Content = hotel.Content,
                Pictures = hotel.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).OrderByDescending(x => x.Size).Select(x => x.Url).ToList(),
                TinhThanh = hotel.IdTinhThanhNavigation.TenTt,
                QuanHuyen = hotel.IdQuanNavigation.Tenquan,
                TinhThanh_url = hotel.IdTinhThanhNavigation.SanitizedName,
                QuanHuyen_url = hotel.IdQuanNavigation.SanitizedName,
                Rooms = _mapper.Map<List<HotelRoom>, List<HotelRoomDto>>(hotel.HotelRooms.ToList()),

                NumberReviewers = hotel.NumberReviewers
            };
            for(int i=0;i< hotel.HotelRooms.Count(); i++)
            {
                hotelDto.Rooms[i].PalletbedText = hotel.HotelRooms.ToList()[i].PalletbedNavigation.Text;
            }    
            var room = hotelDto.Rooms.Where(x => x.Discount != null).MinBy(x => x.NewPrice);
            if (room != null)
            {
                hotelDto.Discount = room.Discount;
                hotelDto.OriginalPrice = (decimal)room.PriceValue;

                hotelDto.ActualPrice = (decimal)room.NewPrice;
            }

            return hotelDto;
        }
    }
}
