
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
    public class HotelServices : IHotelServices
    {
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;
        private readonly ICommonRepository<RoomView> _roomViewsRepository;
        private readonly ICommonRepository<RoomMameniti> _roomServicesRepository;

        private readonly IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public HotelServices(ICommonRepository<Hotel> hotelRepository, ICommonUoW commonUoW, IMapper mapper,
            ICommonRepository<HotelRoom> roomRepository, ICommonRepository<RoomView> roomViewRepository, ICommonRepository<RoomMameniti> roomServicesRepository)
        {
            _hotelRepository = hotelRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _roomViewsRepository = roomViewRepository;
            _roomServicesRepository = roomServicesRepository;
        }

        public ResponseBase GetHotelList(RequestGetListHotel request)
        {
            ResponseBase response = new ResponseBase();

            PagingList<Hotel> hotel = new PagingList<Hotel>();
            if (request.IdProvince == null)
            {
                hotel = _hotelRepository.FindAll(x => x.Deleted!=1).Include(x=>x.IdPriceRangeNavigation)
                    .ToList().ConvertToPaging(request.PageSize, request.PageIndex);
            }
            else
            {
                hotel = _hotelRepository.FindAll(x => x.IdTinhThanh == request.IdProvince&&x.Deleted!=1)
                    .Include(x => x.IdPriceRangeNavigation).ToList().ConvertToPaging(request.PageSize, request.PageIndex);
            }
            var list = _mapper.Map<PagingList<Hotel>,PagingList<HotelDto>>(hotel);
            list.Items.ForEach(x => x.PriceRange = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().IdPriceRangeNavigation.Title));
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
    }
}

