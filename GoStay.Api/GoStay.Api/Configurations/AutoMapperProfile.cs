using AutoMapper;
using GoStay.Common;
using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.Data.ServiceDto;
using GoStay.Data.Ticket;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.News;
using GoStay.DataDto.RatingDto;
using PartnerGostay.Models;

namespace GoStay.Api.Configurations
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<HotelRoom, RoomByHotelDto>().ReverseMap();
            CreateMap<HotelRoom,HotelRoomDto>().ReverseMap();
            CreateMap<GoStay.DataAccess.Entities.Service, ServiceDetailHotelDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();

            CreateMap<OrderDetailDto, OrderDetail>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();

            CreateMap<OrderDetail, InsertOrderDetailDto>().ReverseMap();
            CreateMap<InsertOrderDetailDto, OrderDetail>().ReverseMap();

            CreateMap<OrderDetailShowDto, OrderDetail>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailShowDto>().ReverseMap();

            CreateMap<HotelRoom, HotelRoomOrderDto>().ReverseMap();
            CreateMap<HotelRoomOrderDto, HotelRoom>().ReverseMap();

            CreateMap<Order, OrderGetInfoDto>().ReverseMap();
            CreateMap<OrderGetInfoDto, Order>().ReverseMap();

            CreateMap<TourDetailDto, TourDetail>().ReverseMap();
            CreateMap<TourDetail, TourDetailDto>().ReverseMap();

            CreateMap<Tour, TourOrderDto>().ReverseMap();
            CreateMap<TourOrderDto, Tour>().ReverseMap();

            CreateMap<Tour, TourContentDto>().ReverseMap();
            CreateMap<TourContentDto, Tour>().ReverseMap();

            CreateMap<Banner, BannerDetailDto>().ReverseMap();
            CreateMap<BannerDetailDto, Banner>().ReverseMap();

            CreateMap<TinhThanh, TinhThanhBannerDto>().ReverseMap();
            CreateMap<TinhThanhBannerDto, TinhThanh>().ReverseMap(); 


            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<PagingList<Hotel>, PagingList<HotelDto>>().ReverseMap();

            CreateMap<Quan, QuanDto>().ReverseMap();
            CreateMap<AddRoomModel, HotelRoom>().ReverseMap();
            CreateMap<EditRoomModel, HotelRoom>().ReverseMap();
            
            CreateMap<HotelRoom, RoomDto>().ReverseMap();
            CreateMap<PagingList<HotelRoom>, PagingList<RoomDto>>().ReverseMap();

            CreateMap<ViewDirection, ViewRoomDto>().ReverseMap();
            CreateMap<Service, ServiceRoomDto>().ReverseMap();

            CreateMap<Hotel, HotelListUserDto>().ReverseMap();
            CreateMap<Picture, PictureRoomDto>().ReverseMap();

            CreateMap<OrderTicketDetailDto, OrderTicketDetail>().ReverseMap();
            CreateMap<OrderTicketDto, OrderTicket>().ReverseMap();
            CreateMap<TicketPassengerDto, TicketPassenger>().ReverseMap();

            CreateMap<OrderTicket, OrderTicketShowDto>().ReverseMap();
            CreateMap<OrderTicketDetail, OrderTicketDetailShowDto>().ReverseMap();
            CreateMap<TicketPassenger, TicketPassengerShowDto>().ReverseMap();

            CreateMap<OrderTicket, OrderTicketAdminDto>().ReverseMap();
            CreateMap<PagingList<OrderTicket>, PagingList<OrderTicketAdminDto>>().ReverseMap();

            CreateMap<VideoModel, VideoNews>().ReverseMap();
            CreateMap<HotelRating, RatingAdminDto>().ReverseMap();

            CreateMap<TourStyle, TourStyleDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<TourStartTime, TourStartTimeDto>().ReverseMap();
            CreateMap<TourTopic, TourTopicDto>().ReverseMap();
            CreateMap<TinhThanh, ProvinceDto>().ReverseMap();
            CreateMap<TourRating, TourRatingDto>().ReverseMap();
            CreateMap<Tour, TourAdminDto>().ReverseMap();


        }
    }
}
