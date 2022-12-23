//using AutoMapper;
//using GoStay.Common.Helpers.Hotels;
//using GoStay.Data.HotelDto;
//using GoStay.Data.OrderDto;
//using GoStay.Data.ServiceDto;
//using GoStay.Data.TourDto;
//using GoStay.DataAccess.Entities;
//using GoStay.DataAccess.Interface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GoStay.Common.Helpers.Order
//{
//    public class OrderFunction : IOrderFunction
//    {
//        private readonly IMapper _mapper;
//        private readonly ICommonRepository<Hotel> _hotelRepository;
//        private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
//        private readonly ICommonRepository<Service> _serviceRepository;
//        private readonly ICommonRepository<Picture> _pictureRepository;
//        private readonly ICommonRepository<ViewDirection> _viewRepository;
//        private readonly ICommonRepository<TypeHotel> _typeHotelRepository;
//        private readonly ICommonRepository<TinhThanh> _tinhThanhRepository;
//        private readonly ICommonRepository<OrderDetail> _quanRepository;
//        private readonly ICommonRepository<Phuong> _phuongRepository;


//        public OrderFunction(IMapper mapper)
//        {
//            _mapper = mapper;
//        }
//        public OrderDetailInfoDto CreateOrderDetailInfoDto(OrderDetail orderDetail)
//        {
//            var orderDetailInfoDto = new OrderDetailInfoDto
//            {
//                Id = orderDetail.Id,

//                ChechIn = orderDetail.ChechIn,
//                CheckOut  = orderDetail.CheckOut,
//                Num = orderDetail.Num,
//                Price = orderDetail.Price,
//                Discount = orderDetail.Discount,
//                NewPrice = orderDetail.Price*(100- (decimal)orderDetail.Discount) /100,
//                MoreInfo = orderDetail.MoreInfo,
//            };
//            if (orderDetail.DetailStyle == 1)
//            {
//                orderDetailInfoDto.DetailStyle = "room";
//                orderDetailInfoDto.IdProduct = orderDetail.IdRoom;
//                orderDetailInfoDto.Rooms = _mapper.Map<List<HotelRoom>,List<HotelRoomOrderDto>>(orderDetail)
//            }

//            if (orderDetail.DetailStyle == 2)
//            {
//                orderDetailInfoDto.DetailStyle = "tour";
//                orderDetailInfoDto.IdProduct = orderDetail.IdTour;
//            }
//            var room = hotelDto.Rooms.Where(x => x.Discount != null).MaxBy(x => x.Discount);
//            if (room != null)
//            {
//                hotelDto.Discount = room.Discount;
//                hotelDto.OriginalPrice = (decimal)room.PriceValue;

//                hotelDto.ActualPrice = (decimal)room.NewPrice;
//            }

//            return hotelDto;
//        }
//    }
//}
