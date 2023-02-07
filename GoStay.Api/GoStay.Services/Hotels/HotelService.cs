using AutoMapper;
using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.Base;
using GoStay.Data.Enums;
using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Security.Cryptography.X509Certificates;

namespace GoStay.Services.Hotels
{
	public class HotelService : IHotelService
	{
		private readonly ICommonRepository<Hotel> _hotelRepository;
		private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
        private readonly ICommonRepository<Service> _serviceRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<ViewDirection> _viewRepository;
        private readonly ICommonRepository<TypeHotel> _typeHotelRepository;
        private readonly ICommonRepository<TinhThanh> _tinhThanhRepository;
        private readonly ICommonRepository<Quan> _quanRepository;
        private readonly ICommonRepository<Phuong> _phuongRepository;




        private readonly IMapper _mapper;
		public HotelService(ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelRoom> hotelRoomRepository, IMapper mapper,
			ICommonRepository<Service> serviceRepository, ICommonRepository<Picture> pictureRepository,
			ICommonRepository<ViewDirection> viewRepository, ICommonRepository<TypeHotel> typeHotelRepository, 
			ICommonRepository<TinhThanh> tinhThanhRepository, ICommonRepository<Quan> quanRepository, ICommonRepository<Phuong> phuongRepository)
		{
			_hotelRepository = hotelRepository;
			_hotelRoomRepository = hotelRoomRepository;
			_mapper = mapper;
			_serviceRepository = serviceRepository;
			_pictureRepository = pictureRepository;
			_viewRepository = viewRepository;
			_typeHotelRepository = typeHotelRepository;
			_tinhThanhRepository = tinhThanhRepository;
			_quanRepository = quanRepository;
			_phuongRepository = phuongRepository;
		}
        public ResponseBase GetListHotelTopFlashSale(int number)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var hotels = _hotelRepository.FindAll(x => x.Deleted != 1)
                                                .Include(x => x.Pictures.Take(5))
                                                .Include(x => x.IdTinhThanhNavigation)
                                                .Include(x => x.IdQuanNavigation)
                                                .Include(x => x.HotelRooms.Where(x => x.Status == 1))
                                                .OrderByDescending(x => x.HotelRooms.Max(x => x.Discount))
												.Take(number)
                                                .ToList();
                var hotelDtos = CommonFunction.CreateHotelFlashSaleDto(hotels);
                responseBase.Data = hotelDtos;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

		public ResponseBase GetListRoomByHotel(int hotelId)
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var rooms = _hotelRoomRepository.FindAll(x => x.Deleted != 1 && x.Idhotel == hotelId)
													.Include(x => x.Pictures.Skip(5).Take(5));
				var items = _mapper.Map<List<RoomByHotelDto>>(rooms.ToList());
				foreach (var room in rooms)
				{
					var item = items.Where(x => x.Id == room.Id).FirstOrDefault();
					if (item != null)
						item.Pictures = room.Pictures.Where(x => !string.IsNullOrEmpty(x.Url)).Select(x => x.Url).ToList();
				}
				responseBase.Data = items;
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}


		public ResponseBase GetListForSearchHotel(HotelSearchRequest filter)
		{
			ResponseBase responseBase = new ResponseBase();
			try
			{
                responseBase.Data = HotelRepository.GetPagingListHotelForHomePage(filter);
				return responseBase;
			}
			catch (Exception e)
			{
				responseBase.Code = ErrorCodeMessage.Exception.Key;
				responseBase.Message = e.Message;
				return responseBase;
			}
		}

        public ResponseBase GetListSuggestHotel(string searchText)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
				// Remove unicode
				searchText = searchText.RemoveUnicode();
				searchText = searchText.Replace(" ", string.Empty).ToLower();
                var listData = HotelRepository.GetListLocationForDropdown(searchText);
				foreach (var item in listData.Where(x=>x.Type== LocationDropdown.Hotel))
				{
					item.HotelTypeName = _typeHotelRepository.FindAll().SingleOrDefault(x => x.Id == item.HotelType).Type;
					item.NumRecord = 0;
                }
                responseBase.Data = listData;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase GetHotelDetail(int hotelId)
		{
			IHotelFunction hotelFunction= new HotelFunction(_mapper) ;
			ResponseBase responseBase = new ResponseBase();
			try
			{
				var hotel = _hotelRepository.FindAll(x => x.Id == hotelId && x.Deleted != 1)
					.Include(x=>x.HotelRooms.Where(x=>x.Deleted!=1).OrderByDescending(x=>x.Discount).OrderByDescending(x=>x.RemainNum))
					.Include(x=>x.Pictures.Where(x=>x.Deleted==0).Take(5))
					.Include(x=>x.IdQuanNavigation)
					.Include(x=>x.IdTinhThanhNavigation)
					.Include(x=>x.HotelMamenitis)
					.FirstOrDefault();
                List<Service> svhotel = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == 0)
                                                .Where(x => x.HotelMamenitis.Any(x => x.Idhotel == hotelId))
                                                .OrderBy(x => x.Name).OrderBy(x=>x.AdvantageLevel).Take(4).ToList();


				var hotelRoom = _hotelRoomRepository.FindAll(x => x.Idhotel == hotelId)
										.Include(x=>x.RoomMamenitis)
										.Include(x=>x.ViewDirectionNavigation)
										.Include(x=>x.PalletbedNavigation)
										.Include(x=>x.Pictures.Take(4)).ToList();
                if (hotel==null)
				{
                    responseBase.Message= ErrorCodeMessage.Exception.Value;
                    responseBase.Code = ErrorCodeMessage.Exception.Key;
                    return responseBase;
				}

                var hotelDetailDto = hotelFunction.CreateHotelDetailDto(hotel);
				hotelDetailDto.Services = _mapper.Map<List<Service>,List<ServiceDetailHotelDto>>(svhotel);
				
                for (int i=0; i< hotelDetailDto.Rooms.Count;i++)
                {
					hotelDetailDto.Rooms[i].Pictures = hotelRoom[i].Pictures.Select(x=>x.Url).ToList();
					hotelDetailDto.Rooms[i].Services = _mapper.Map<List<Service>, List<ServiceDetailHotelDto>>
												(_serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == 1)
												.Where(x => x.RoomMamenitis.Any(x => x.Idroom == hotelDetailDto.Rooms[i].Id))
												.OrderBy(x => x.Name).OrderBy(x => x.AdvantageLevel).Take(5).ToList());
					hotelDetailDto.Rooms[i].ViewDirection = _viewRepository.GetById(hotelRoom[i].ViewDirection)?.ViewDirection1;
                }

                responseBase.Data = hotelDetailDto;
                return responseBase;
            }
			catch
			{
                var hotelDetailDto = new HotelDetailDto();
                responseBase.Data = hotelDetailDto;
                return responseBase;
            }
        }
	}
}
