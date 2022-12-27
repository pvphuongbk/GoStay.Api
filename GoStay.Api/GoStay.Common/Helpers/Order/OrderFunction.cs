using AutoMapper;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.Data.ServiceDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
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
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Service> _serviceRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<ViewDirection> _viewRepository;

        private readonly ICommonRepository<Palletbed> _palletbedRepository;


        public OrderFunction(IMapper mapper, ICommonRepository<Hotel> hotelRepository, ICommonRepository<Service> serviceRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<ViewDirection> viewRepository, ICommonRepository<Palletbed> palletbedRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _serviceRepository = serviceRepository;
            _pictureRepository = pictureRepository;
            _viewRepository = viewRepository;
            _palletbedRepository = palletbedRepository;
        }
        public OrderDetailInfoDto CreateOrderDetailInfoDto(OrderDetail orderDetail)
        {
            var orderDetailInfoDto = new OrderDetailInfoDto
            {
                Id = orderDetail.Id,

                ChechIn = orderDetail.ChechIn,
                CheckOut = orderDetail.CheckOut,
                DateCreate = orderDetail.DateCreate,
                Num = orderDetail.Num,
                Price = orderDetail.Price,
                Discount = orderDetail.Discount,
                NewPrice = orderDetail.Price * (100 - (decimal)orderDetail.Discount) / 100,
                MoreInfo = orderDetail.MoreInfo,
            };
            if (orderDetail.DetailStyle == 1)
            {
                orderDetailInfoDto.DetailStyle = "room";
                orderDetailInfoDto.IdProduct = orderDetail.IdRoom;
                orderDetailInfoDto.Rooms = CreateHotelRoomOrderDto(orderDetail.IdRoomNavigation);
            }

            if (orderDetail.DetailStyle == 2)
            {
                orderDetailInfoDto.DetailStyle = "tour";
                orderDetailInfoDto.IdProduct = orderDetail.IdTour;
            }
            return orderDetailInfoDto;
        }

        public HotelRoomOrderDto CreateHotelRoomOrderDto(HotelRoom roomOrderDetail)
        {
            var hotelRoomOrderDto = _mapper.Map<HotelRoom,HotelRoomOrderDto>(roomOrderDetail);
            var hotel = _hotelRepository.GetById(roomOrderDetail.Idhotel);
            hotelRoomOrderDto.HotelName = hotel.Name;
            hotelRoomOrderDto.Address = hotel.Address;
            hotelRoomOrderDto.Rating = hotel.Rating;
            hotelRoomOrderDto.ReviewScore = hotel.ReviewScore;
            hotelRoomOrderDto.NumberReviewers = hotel.NumberReviewers;
            hotelRoomOrderDto.ViewDirection = _viewRepository.GetById(roomOrderDetail.ViewDirection).ViewDirection1;
            hotelRoomOrderDto.Pictures = _pictureRepository.FindAll(x => x.HotelRoomId == roomOrderDetail.Id && x.Type==1).Select(x=>x.Url).Take(1).ToList();
            hotelRoomOrderDto.Pictures.AddRange(_pictureRepository.FindAll(x => x.HotelId == hotel.Id && x.Type == 0).Select(x => x.Url).Take(1).ToList());
            hotelRoomOrderDto.PalletbedText = _palletbedRepository.GetById(roomOrderDetail.Palletbed).Text;
            hotelRoomOrderDto.Services = _mapper.Map<List<Service>, List<ServiceDetailHotelDto>>
                                                    (_serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == 1)
                                                    .Where(x => x.RoomMamenitis.Any(x => x.Idroom == roomOrderDetail.Id))
                                                    .OrderBy(x => x.Name).OrderBy(x => x.AdvantageLevel).Take(5).ToList());
            return hotelRoomOrderDto;
        }
    }
}
