using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Common.Helpers.Order
{
    public interface IOrderFunction
    {
        public OrderDetailInfoDto CreateOrderDetailInfoDto(OrderDetail orderDetail);
        public HotelRoomOrderDto CreateHotelRoomOrderDto(HotelRoom roomOrderDetail);

    }
}
