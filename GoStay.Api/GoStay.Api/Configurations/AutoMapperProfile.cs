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

        }
    }
}
