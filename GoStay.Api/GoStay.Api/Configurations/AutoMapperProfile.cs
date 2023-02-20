using AutoMapper;
using GoStay.Common;
using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.Data.ServiceDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Banner;
using GoStay.DataDto.Hành_Chính;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using System.Linq;

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

            CreateMap<RoomAddDto, HotelRoom>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<PagingList<Hotel>, PagingList<HotelDto>>().ReverseMap();


        }
    }
}
