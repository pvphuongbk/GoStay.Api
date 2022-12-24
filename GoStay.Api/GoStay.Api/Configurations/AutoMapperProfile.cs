using AutoMapper;
using GoStay.Data.HotelDto;
using GoStay.Data.OrderDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using System.Linq;

namespace GoStay.Api.Configurations
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<HotelRoom, RoomByHotelDto>().ReverseMap();
            CreateMap<HotelRoom,HotelRoomDto>().ReverseMap();
            CreateMap<Service, ServiceDetailHotelDto>().ReverseMap();

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


        }
    }
}
