using AutoMapper;
using GoStay.Data.HotelDto;
using GoStay.DataAccess.Entities;

namespace GoStay.Api.Configurations
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<HotelRoom, RoomByHotelDto>().ReverseMap();
		}
	}
}
