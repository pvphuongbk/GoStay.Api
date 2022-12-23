using AutoMapper;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helpers.Order
{
    public class OrderFunction : IOrderFunction
    {
        private readonly IMapper _mapper;

        public OrderFunction(IMapper mapper)
        {
            _mapper = mapper;
        }
        //public OrderDetailInfoDto CreateOrderDetailInfoDto(Hotel hotel)
        //{
        //    var orderDetailInfoDto = new OrderDetailInfoDto
        //    {
        //        Id = hotel.Id,

        //    };
            
            //var room = hotelDto.Rooms.Where(x => x.Discount != null).MaxBy(x => x.Discount);
            //if (room != null)
            //{
            //    hotelDto.Discount = room.Discount;
            //    hotelDto.OriginalPrice = (decimal)room.PriceValue;

            //    hotelDto.ActualPrice = (decimal)room.NewPrice;
            //}

            //return hotelDto;
        //}
    }
}
