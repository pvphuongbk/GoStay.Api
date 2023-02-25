
using AutoMapper;
using GoStay.Common;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ErrorCodeMessage = GoStay.Common.ErrorCodeMessage;

namespace GoStay.Services.WebSupport
{
    public class HotelService : IHotelService
    {
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;
        private readonly ICommonRepository<RoomView> _roomViewsRepository;
        private readonly ICommonRepository<RoomMameniti> _roomServicesRepository;
        private readonly ICommonRepository<ViewDirection> _viewRepository;
        private readonly ICommonRepository<Palletbed> _palletbedRepository;
        private readonly ICommonRepository<Service> _servicesRepository;


        private readonly IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public HotelService(ICommonRepository<Hotel> hotelRepository, ICommonUoW commonUoW, IMapper mapper,
            ICommonRepository<HotelRoom> roomRepository, ICommonRepository<RoomView> roomViewRepository, ICommonRepository<RoomMameniti> roomServicesRepository,
            ICommonRepository<ViewDirection> viewRepository, ICommonRepository<Palletbed> palletbedRepository, ICommonRepository<Service> servicesRepository)
        {
            _hotelRepository = hotelRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _roomViewsRepository = roomViewRepository;
            _roomServicesRepository = roomServicesRepository;
            _viewRepository = viewRepository;
            _palletbedRepository = palletbedRepository;
            _servicesRepository = servicesRepository;
        }

        public ResponseBase GetHotelList(RequestGetListHotel request)
        {
            ResponseBase response = new ResponseBase();
            if(request.NameSearch == null)
            {
                request.NameSearch = "";
            }    
            request.NameSearch = request.NameSearch.RemoveUnicode();
            request.NameSearch = request.NameSearch.Replace(" ", string.Empty).ToLower();
            PagingList<Hotel> hotel = new PagingList<Hotel>();
            if (request.IdProvince == null|| request.IdProvince ==0)
            {
                hotel = _hotelRepository.FindAll(x => x.Deleted!=1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x=>x.IdPriceRangeNavigation).Include(x=>x.TypeNavigation)
                    .ToList().ConvertToPaging(request.PageSize??10, request.PageIndex??1);
            }
            else
            {
                hotel = _hotelRepository.FindAll(x => x.IdTinhThanh == request.IdProvince&&x.Deleted!=1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x => x.IdPriceRangeNavigation).Include(x => x.TypeNavigation)
                    .ToList().ConvertToPaging(request.PageSize??10, request.PageIndex??1);
            }
            var list = _mapper.Map<PagingList<Hotel>,PagingList<HotelDto>>(hotel);
            list.Items.ForEach(x => x.PriceRange = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().IdPriceRangeNavigation.Title));
            list.Items.ForEach(x => x.TypeHotel = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().TypeNavigation.Type));

            response.Data = list;
            return response;
        }
        public ResponseBase AddRoom(HotelRoom data, int[] view, int[] service)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                _roomRepository.Insert(data);
                _commonUoW.Commit();
                if (view != null)
                {
                    foreach (var item in view)
                    {
                        _commonUoW.BeginTransaction();
                        _roomViewsRepository.Insert(new RoomView() { IdRoom = data.Id, IdView = item });
                        _commonUoW.Commit();
                    }
                }
                if (service != null)
                {
                    foreach (var item in service)
                    {
                        _commonUoW.BeginTransaction();
                        _roomServicesRepository.Insert(new RoomMameniti() { Idroom = data.Id, Idservices = item });
                        _commonUoW.Commit();
                    }
                }
                response.Message= $"{ErrorCodeMessage.Success.Value}";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = data.Id;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = $"{ErrorCodeMessage.AddFail.Value}";
                response.Code = ErrorCodeMessage.AddFail.Key;
                return response;
            }
        }

        public ResponseBase SupportAddRoom()
        {
            ResponseBase response = new ResponseBase();
            SupportAddRoom support = new SupportAddRoom();
            support.views = _viewRepository.FindAll().ToList();
            support.palletbed = _palletbedRepository.FindAll().ToList();
            support.servicesRoom = _servicesRepository.FindAll(x=>x.IdStyle==1).ToList();

            response.Data = support;
            return response;
        }
    }
}

