﻿using AutoMapper;
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

        private readonly ICommonRepository<TourStyle> _tourStyleRepository;
        private readonly ICommonRepository<TourTopic> _tourTopicRepository;
        private readonly ICommonRepository<TourDetail> _tourDetailRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourProvinceToRepository;

        private readonly ICommonRepository<TinhThanh> _tinhThanhRepository;
        private readonly ICommonRepository<User> _userRepository;

        public OrderFunction(IMapper mapper, ICommonRepository<Hotel> hotelRepository, ICommonRepository<Service> serviceRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<ViewDirection> viewRepository,
            ICommonRepository<Palletbed> palletbedRepository, ICommonRepository<TourStyle> tourStyleRepository, 
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TinhThanh> tinhThanhRepository,
            ICommonRepository<User> userRepository, ICommonRepository<TourDetail> tourDetailRepository, ICommonRepository<TourDistrictTo> tourProvinceToRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
            _serviceRepository = serviceRepository;
            _pictureRepository = pictureRepository;
            _viewRepository = viewRepository;
            _palletbedRepository = palletbedRepository;
            _tourStyleRepository = tourStyleRepository;
            _tourTopicRepository = tourTopicRepository;
            _tinhThanhRepository = tinhThanhRepository;
            _userRepository = userRepository;
            _tourDetailRepository = tourDetailRepository;
            _tourProvinceToRepository = tourProvinceToRepository;
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
                MoreInfo = orderDetail.MoreInfo,
            };
            if (orderDetail.DetailStyle == 1)
            {
                orderDetailInfoDto.DetailStyle = "room";
                orderDetailInfoDto.IdProduct = orderDetail.IdRoom;
                orderDetailInfoDto.Rooms = CreateHotelRoomOrderDto(orderDetail.IdRoomNavigation);
                orderDetailInfoDto.Price = orderDetailInfoDto.Rooms.PriceValue;
                orderDetailInfoDto.Discount = orderDetailInfoDto.Rooms.Discount;
                orderDetailInfoDto.NewPrice = orderDetailInfoDto.Price * (100 - (decimal)orderDetailInfoDto.Discount) / 100;

            }

            if (orderDetail.DetailStyle == 2)
            {
                orderDetailInfoDto.DetailStyle = "tour";
                orderDetailInfoDto.IdProduct = orderDetail.IdTour;
                orderDetailInfoDto.Tours = CreateTourOrderDto(orderDetail.IdTourNavigation);
                orderDetailInfoDto.Price = (decimal)orderDetailInfoDto.Tours.Price;
                orderDetailInfoDto.Discount = orderDetailInfoDto.Tours.Discount;
                orderDetailInfoDto.NewPrice = orderDetailInfoDto.Price * (100 - (decimal)orderDetailInfoDto.Discount) / 100;
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
            hotelRoomOrderDto.ReviewScore = (int?)hotel.ReviewScore;
            hotelRoomOrderDto.NumberReviewers = hotel.NumberReviewers;
            hotelRoomOrderDto.ViewDirection = roomOrderDetail.RoomViews.FirstOrDefault().IdViewNavigation.ViewDirection1;
            hotelRoomOrderDto.Pictures = _pictureRepository.FindAll(x => x.HotelRoomId == roomOrderDetail.Id && x.Type==1)?.Select(x=>x.Url).Take(1).ToList();
            hotelRoomOrderDto.Pictures.AddRange(_pictureRepository.FindAll(x => x.HotelId == hotel.Id && x.Type == 0)?.Select(x => x.Url).Take(1).ToList());
            hotelRoomOrderDto.PalletbedText = roomOrderDetail.PalletbedNavigation.Text;

            hotelRoomOrderDto.Services = _mapper.Map<List<Service>, List<ServiceDetailHotelDto>>
                                        (roomOrderDetail.RoomMamenitis.Select(x => x.IdservicesNavigation).ToList());

            return hotelRoomOrderDto;
        }
        public TourOrderDto CreateTourOrderDto(Tour tourOrderDetail)
        {
            var tourOrderDto = _mapper.Map<Tour, TourOrderDto>(tourOrderDetail);
            tourOrderDto.TourStyle = tourOrderDetail.IdTourStyleNavigation.TourStyle1;
            tourOrderDto.TourTopic = tourOrderDetail.IdTourTopicNavigation.TourTopic1;
            tourOrderDto.UserName = _userRepository.GetById(tourOrderDto.IdUser)?.UserName;
            tourOrderDto.ProvinceFrom = tourOrderDetail.IdDistrictFromNavigation.IdTinhThanhNavigation.TenTt;
            tourOrderDto.StartTime = tourOrderDetail.IdStartTimeNavigation.StartDate;
            tourOrderDto.Pictures = _pictureRepository.FindAll(x => x.TourId == tourOrderDetail.Id && x.Type == 2)?.Select(x => x.Url).Take(2).ToList();
            var listTourDetail = tourOrderDetail.TourDetails.ToList();

            tourOrderDto.TourDetails = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(listTourDetail);
            var listprovinceto = new List<string>();
            foreach (var item in tourOrderDetail.TourDistrictTos.Select(x=>x.IdDistrictToNavigation.IdTinhThanhNavigation.TenTt))
            {
                listprovinceto.Add(item);
            }
            tourOrderDto.ProvinceTo = listprovinceto;
            return tourOrderDto;
        }
    }
}
