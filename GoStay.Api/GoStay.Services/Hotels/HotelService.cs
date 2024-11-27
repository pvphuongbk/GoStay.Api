using AutoMapper;
using GoStay.Common.Constants;
using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.Base;
using GoStay.Data.Enums;
using GoStay.Data.HotelDto;
using GoStay.Data.ServiceDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.UnitOfWork;
using GoStay.DataDto.HotelDto;
using GoStay.DataDto.HotelFlashSales;
using GoStay.Repository.DapperHelper;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace GoStay.Services.Hotels
{
	public class HotelService : IHotelService
	{
		private readonly ICommonRepository<Hotel> _hotelRepository;
		private readonly ICommonRepository<HotelRoom> _hotelRoomRepository;
        private readonly ICommonRepository<Service> _serviceRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<TypeHotel> _typeHotelRepository;
        private readonly ICommonRepository<HotelFlashSale> _hotelFlashSaleRepository;

        private readonly ICommonRepository<SchedulerRoomPrice> _schedulerRepository;
        private readonly CommonUoW _commonUoWRepository;



        private readonly IMapper _mapper;
		public HotelService(ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelRoom> hotelRoomRepository, IMapper mapper,
			ICommonRepository<Service> serviceRepository, ICommonRepository<Picture> pictureRepository, ICommonRepository<TypeHotel> typeHotelRepository,
            ICommonRepository<SchedulerRoomPrice> schedulerRepository, ICommonUoW commonUoWRepository, ICommonRepository<HotelFlashSale> hotelFlashSaleRepository)
		{
			_hotelRepository = hotelRepository;
			_hotelRoomRepository = hotelRoomRepository;
			_mapper = mapper;
			_serviceRepository = serviceRepository;
			_pictureRepository = pictureRepository;
			_typeHotelRepository = typeHotelRepository;
            _schedulerRepository = schedulerRepository;
            _commonUoWRepository = (CommonUoW)commonUoWRepository;
            _hotelFlashSaleRepository = hotelFlashSaleRepository;
        }
        public ResponseBase GetHotelFlashSalePresentData()
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var data = new HotelFlashSalePresentViewModel();
                var listPresent = _hotelFlashSaleRepository.FindAll(x => x.Deleted == false).OrderByDescending(x=>x.IsPin)
                    .Select(x=> new HotelFlashSaleObjectModel
                    {
                        HotelId = x.HotelId,
                        IsPin = x.IsPin,
                        Name = "",
                        PictureUrl = ""
                    }).ToList();
                var listPresentId= listPresent.Select(x=>x.HotelId).ToList();

                var listHotelPresent = _hotelRepository.FindAll(x => listPresentId.Contains(x.Id)).Include(x => x.Pictures)
                        .Select(x=> new HotelFlashSaleForSelection
                        {
                            Id = x.Id,
                            Name = x.Name,
                            PictureUrl = "https://cdn.realtech.com.vn"+x.Pictures.FirstOrDefault().Url??string.Empty
                        }).ToList();
                foreach (var item in listPresent)
                {
                    var hotel = listHotelPresent.FirstOrDefault(x => x.Id == item.HotelId);
                    item.Name = hotel.Name??"";
                    item.PictureUrl = hotel.PictureUrl?? "";
                }

                data.ListPresent = listPresent;
                data.Total= listHotelPresent.Count();
                data.PageIndex = 1;
                data.PageSize= listHotelPresent.Count();
                responseBase.Data = data;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetHotelFlashSaleSelectionData(int pageIndex, int pageSize, string? keyword)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var data = new HotelFlashSaleSelectionViewModel();
                var listPresentId = _hotelFlashSaleRepository.FindAll(x => x.Deleted == false).Select(x => x.HotelId).ToList();

                var temp = _hotelRepository.FindAll(x => !listPresentId.Contains(x.Id) && x.Deleted != 1 && (!keyword.IsNullOrEmpty()? x.SearchKey.Contains(keyword):x.Id>0)).Include(x => x.Pictures);
                data.Total = temp.Count();
                data.PageIndex = pageIndex;
                data.PageSize = pageSize;

                var listForSelection = temp.Skip((pageIndex - 1)*pageSize).Take(pageSize).Select(x => new HotelFlashSaleForSelection
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PictureUrl = "https://cdn.realtech.com.vn" + x.Pictures.FirstOrDefault().Url ?? string.Empty
                    }).ToList();
                data.ListSelection = listForSelection;
                responseBase.Data = data;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public async Task<ResponseBase> UpsertHotelTopFlashSale(List<HotelFlashSaleUpsertRequestModel> requestModel)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if(!requestModel.Any())
                {
                    responseBase.Message = "Request param empty";
                    return responseBase;
                }
                if(requestModel.Where(x=>x.IsPin==true).Count()>3)
                {
                    responseBase.Message = "You can pin max 3 hotel";
                    return responseBase;
                }
                var listId = _hotelRepository.FindAll(x => x.Deleted != 1).Select(x => x.Id).ToList();
                if(requestModel.Any(x=> !listId.Contains(x.HotelId)))
                {
                    responseBase.Message = "One or more Hotel not existing";
                    return responseBase;
                }    
                var listPresent = _hotelFlashSaleRepository.FindAll();
                var listPresentId = listPresent.Select(x=>x.HotelId);
                //insert

                var requestNew = requestModel.Where(x => !listPresentId.Contains(x.HotelId)).ToList();
                var requestOld = requestModel.Where(x => listPresentId.Contains(x.HotelId)).ToList();
                var entitiesNew = requestNew.Select(x => new HotelFlashSale
                {
                    HotelId = x.HotelId,
                    IsPin = x.IsPin,
                    Deleted=false,
                }).ToList();
                _commonUoWRepository.BeginTransaction();
                _hotelFlashSaleRepository.InsertMultiple(entitiesNew);
                _commonUoWRepository.Commit();
                var requestNewIds = entitiesNew.Select(x=>x.HotelId).ToList();
                //update
                var requestIds = requestOld.Select(x => x.HotelId).ToList();
                var requestPinsId = requestOld.Where(x => x.IsPin == true).Select(x => x.HotelId).ToList();

                var listOff = listPresent.Where(x => !requestIds.Contains(x.HotelId)&& !requestNewIds.Contains(x.HotelId));
                var listOn = listPresent.Where(x => requestIds.Contains(x.HotelId) && !requestNewIds.Contains(x.HotelId));
                await listOff.ForEachAsync(x => x.Deleted = true);
                await listOn.ForEachAsync(x => x.IsPin = false);
                await listOn.Where(x=> requestPinsId.Contains(x.HotelId)).ForEachAsync(x => x.IsPin = true);
                _commonUoWRepository.BeginTransaction();
                _hotelFlashSaleRepository.UpdateMultiple(listOff);
                _hotelFlashSaleRepository.UpdateMultiple(listOn);
                _commonUoWRepository.Commit();

                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpsertHotelFlashSale(HotelFlashSaleUpsertRequestModel requestModel)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (requestModel==null)
                {
                    responseBase.Message = "Request param empty";
                    return responseBase;
                }
                //if (requestModel.Where(x => x.IsPin == true).Count() > 3)
                //{
                //    responseBase.Message = "You can pin max 3 hotel";
                //    return responseBase;
                //}
                var listId = _hotelRepository.FindAll(x => x.Deleted != 1).Select(x => x.Id).ToList();
                if ( !listId.Contains(requestModel.HotelId))
                {
                    responseBase.Message = "One or more Hotel not existing";
                    return responseBase;
                }
                var listPresent = _hotelFlashSaleRepository.FindAll();
                var listPresentId = listPresent.Select(x => x.HotelId);
                var entity = new HotelFlashSale()
                {
                    HotelId = requestModel.HotelId
                };
                if(listPresentId.Contains(requestModel.HotelId))
                    entity = _hotelFlashSaleRepository.FindAll(x=>x.HotelId == requestModel.HotelId).FirstOrDefault();

                entity.IsPin=requestModel.IsPin;
                entity.Deleted = requestModel.IsDelete;

                _commonUoWRepository.BeginTransaction();
                _hotelFlashSaleRepository.Update(entity);
                _commonUoWRepository.Commit();
                responseBase.Data = "Succcess";
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetListHotelTopFlashSale(int number)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listFs = _hotelFlashSaleRepository.FindAll(x => x.Deleted == false).ToList();
                var listFsIds = listFs.Select(x => x.HotelId).ToList();

                var listPinIds = listFs.Where(x=>x.IsPin).Select(x=>x.HotelId).ToList();
                var listUnPinIds = listFs.Where(x => !x.IsPin).Select(x => x.HotelId).ToList();
                if (listFs.Count() >= number)
                    number = 0;
                else
                    number -= listFs.Count();

                var listId = _hotelRepository.FindAll(x => x.Deleted != 1&& !listFsIds.Contains(x.Id)).Select(x => x.Id).ToList();
                var listfinal = CommonFunction.GetRandomFromList(listId, number);
                listUnPinIds.AddRange(listfinal);

                var hotelsPin = _hotelRepository.FindAll(x => listPinIds.Contains(x.Id))
                                                .Include(x => x.Pictures.Take(5))
                                                .Include(x => x.IdTinhThanhNavigation)
                                                .Include(x => x.IdQuanNavigation)
                                                .Include(x => x.HotelRooms.Where(x => x.Status == 1 && x.Deleted != 1))
                                                .AsNoTracking().ToList();
                var hotelsUnPin = _hotelRepository.FindAll(x => listUnPinIds.Contains(x.Id))
                                                .Include(x => x.Pictures.Take(5))
                                                .Include(x => x.IdTinhThanhNavigation)
                                                .Include(x => x.IdQuanNavigation)
                                                .Include(x => x.HotelRooms.Where(x => x.Status == 1 && x.Deleted != 1))
                                                //.OrderByDescending(x => x.HotelRooms.Max(x => x.Discount))
                                                //.Take(number)
                                                .AsNoTracking().ToList();
                
                var hotelDtos = CommonFunction.CreateHotelFlashSaleDto(hotelsPin);
                var hotelsUnPinDto = CommonFunction.CreateHotelFlashSaleDto(hotelsUnPin);

                hotelDtos.AddRange(hotelsUnPinDto);

                Dictionary<int, IQueryable<SchedulerRoomPrice>> roomSchedulers = new Dictionary<int, IQueryable<SchedulerRoomPrice>>();

                foreach (var id in hotelDtos.Select(x => x.IdRoom))
                {
                    var schedulerRoomPrice = _schedulerRepository.FindAll(x => x.RoomId == id && x.Start.Year <= DateTime.Now.Year && x.Start.Month <= DateTime.Now.Month);
                    if (schedulerRoomPrice.Count() > 0)
                    {
                        roomSchedulers.Add(id, schedulerRoomPrice);
                    }
                }
                foreach (var item in roomSchedulers)
                {
                    hotelDtos.Where(x => x.IdRoom == item.Key).SingleOrDefault().DailyPrice =
                        SchedulerRepository.GetPrice(item.Value, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }    

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
        public ResponseBase GetListHotelHomePage(int IdProvince)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (IdProvince == 0)
                {
                    HotelSearchRequest filter = new HotelSearchRequest() { PageIndex = 1, PageSize = 10 };
                    var Data = HotelRepository.GetListHotelHomepage(filter);
                            
                    Data.ForEach(x => x.Slug = (x.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));

                    Dictionary<int, IQueryable<SchedulerRoomPrice>> roomSchedulers = new Dictionary<int, IQueryable<SchedulerRoomPrice>>();

                    foreach (var id in Data.Select(x => x.IdRoom))
                    {
                        var schedulerRoomPrice = _schedulerRepository.FindAll(x => x.RoomId == id && x.Start.Year <= DateTime.Now.Year && x.Start.Month <= DateTime.Now.Month);
                        if (schedulerRoomPrice.Count() > 0)
                        {
                            roomSchedulers.Add(id, schedulerRoomPrice);
                        }
                    }
                    foreach (var item in roomSchedulers)
                    {
                        Data.Where(x => x.IdRoom == item.Key).SingleOrDefault().DailyPrice =
                            SchedulerRepository.GetPrice(item.Value, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    }
                    responseBase.Data = Data;

                    return responseBase;
                }
                else
                {
                    HotelSearchRequest filter = new HotelSearchRequest() { PageIndex = 1, PageSize = 10, IdTinhThanh = IdProvince};
                    var Data = HotelRepository.GetListHotelHomepage(filter);
                    Data.ForEach(x => x.Slug = (x.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
                    responseBase.Data = Data;
                    return responseBase;
                }
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
                var Data = HotelRepository.GetPagingListHotelForHomePage(filter);
                Data.ForEach(x => x.Slug = (x.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
 
                responseBase.Data = Data;
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
                listData.ForEach(x=>x.Slug = x.Value.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                            .Replace("(", string.Empty).Replace(")", string.Empty)
                            .Replace("*", string.Empty).Replace("%", string.Empty)
                            .Replace("&", "-").Replace("@", string.Empty).ToLower());
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
        public ResponseBase GetListNearHotel(int NumTop,float Lat, float Lon)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listData = HotelRepository.GetListNearHotel( NumTop,Lat, Lon);
                listData.ForEach(x => x.Slug = x.Value.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                            .Replace("(", string.Empty).Replace(")", string.Empty)
                            .Replace("*", string.Empty).Replace("%", string.Empty)
                            .Replace("&", "-").Replace("@", string.Empty).ToLower());
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

        public ResponseBase GetHotelDetailNew(int hotelId, int userId)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var dto = HotelDapperExtensions.GetHotelDetailDto(Procedures.sq_GetHotelDetailById, hotelId, userId);
                responseBase.Data = dto;
                return responseBase;
            }
            catch
            {
                var hotelDetailDto = new HotelDetailDto();
                responseBase.Data = hotelDetailDto;
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
					.Include(x=>x.HotelRooms.Where(x=>x.Deleted!=1 && x.Status ==1)
					.OrderByDescending(x=>x.Discount).OrderByDescending(x=>x.RemainNum))
						.ThenInclude(x=>x.RoomMamenitis.OrderByDescending(x=>x.Level).Take(5)).ThenInclude(x=>x.IdservicesNavigation)
						.Include(x=>x.HotelRooms)
						.ThenInclude(x=>x.RoomViews).ThenInclude(x=>x.IdViewNavigation)
                        .Include(x => x.HotelRooms).ThenInclude(x => x.PalletbedNavigation)
                        .Include(x => x.HotelRooms).ThenInclude(x => x.Pictures.Where(x=>x.Deleted==0).Take(4))
                    .Include(x=>x.Pictures.Where(x=>x.Deleted==0).Take(5))
					.Include(x=>x.IdQuanNavigation)
					.Include(x=>x.IdTinhThanhNavigation)
					.Include(x=>x.HotelMamenitis.OrderByDescending(x => x.Level).Take(4)).ThenInclude(x=>x.IdservicesNavigation)
					.FirstOrDefault();
                var svhotel = hotel.HotelMamenitis.Select(x=>x.IdservicesNavigation).ToList();

				var hotelRoom = hotel.HotelRooms.ToList();
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
						(hotelRoom[i].RoomMamenitis.Select(x => x.IdservicesNavigation).ToList())
												;
                }
                hotelDetailDto.TotalPicture = _pictureRepository.FindAll(x=>x.HotelId== hotelId && x.Deleted!=1).Count();
                hotelDetailDto.Slug = hotelDetailDto.HotelName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower();
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

        public ResponseBase GetAllTypeHotel()
        {
            ResponseBase responseBase = new ResponseBase();
            var listTypeHotel = _typeHotelRepository.FindAll(x => x.Deleted != 1).ToList();
			responseBase.Data = listTypeHotel;
            return responseBase;
        }

        public ResponseBase GetServicesSearch(int type)
        {
            ResponseBase responseBase = new ResponseBase();

            var listService = _serviceRepository.FindAll(x => x.Deleted != 1 && x.IdStyle == type)
                                                                .OrderBy(x => x.AdvantageLevel).Take(30).ToList();
            responseBase.Data=listService;
            return responseBase;

        }
    }
}
