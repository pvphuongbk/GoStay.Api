
using AutoMapper;
using GoStay.Common;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using GoStay.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<Album> _albumRepository;

        private readonly IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public HotelService(ICommonRepository<Hotel> hotelRepository, ICommonUoW commonUoW, IMapper mapper,
            ICommonRepository<HotelRoom> roomRepository, ICommonRepository<RoomView> roomViewRepository, ICommonRepository<RoomMameniti> roomServicesRepository,
            ICommonRepository<ViewDirection> viewRepository, ICommonRepository<Palletbed> palletbedRepository, ICommonRepository<Service> servicesRepository
            , ICommonRepository<Picture> pictureRepository, ICommonRepository<Album> albumRepository)
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
            _pictureRepository = pictureRepository;
            _albumRepository = albumRepository;
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
        public ResponseBase AddRoom(HotelRoom data)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                _roomRepository.Insert(data);
                _commonUoW.Commit();

                response.Message= "Add Room Success";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = data.Id;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Add Room Fail";
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
        public ResponseBase AddViewRoom(int IdRoom, List<int> idViews)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                foreach (var id in idViews)
                {
                    _commonUoW.BeginTransaction();
                    _roomViewsRepository.Insert(new RoomView() { IdRoom = IdRoom, IdView = id });
                    _commonUoW.Commit();
                }

                response.Message = "Add View Success";
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Add View Fail";
                response.Code = ErrorCodeMessage.AddFail.Key;
                return response;
            }
        }
        public ResponseBase AddServiceRoom(int IdRoom, List<int> idServices)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                foreach (var id in idServices)
                {
                    _commonUoW.BeginTransaction();
                    _roomServicesRepository.Insert(new RoomMameniti() { Idroom = IdRoom, Idservices = id });
                    _commonUoW.Commit();
                }

                response.Message = "Add Service Success";
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Add Services Fail";
                response.Code = ErrorCodeMessage.AddFail.Key;
                return response;
            }
        }
        public ResponseBase AddAlbumRoom(int IdRoom, string albumName)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var checkName = _albumRepository.FindAll(x => x.Name == albumName && x.IdType == IdRoom);
                if (checkName != null&& checkName.Count()<1)
                {
                    var data = new Album() { Name = albumName, IdRoom= IdRoom, TypeAlbum = 1 };
                    _albumRepository.Insert(data);
                    _commonUoW.Commit();
                    response.Data = data.Id;
                    response.Message = "Add Album Success";
                    response.Code = ErrorCodeMessage.Success.Key;
                }
                else
                {
                    var date = DateTime.Today.ToString("dd/MM/yyyy hh:mm");
                    var data = new Album() { Name = $"AlbumRoom{IdRoom}{date}", IdRoom = IdRoom, TypeAlbum = 1 };
                    _albumRepository.Insert(data);
                    _commonUoW.Commit();
                    response.Data = data;
                    response.Message = "Album Name Existed";
                    response.Code = ErrorCodeMessage.Success.Key;
                }    
                
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Add Album Fail";
                response.Code = ErrorCodeMessage.AddFail.Key;
                return response;
            }
        }
        public ResponseBase AddPictureRoom(int Obj, int IdAlbum, int type, int userId , UploadImagesResponse imagesResponse)
        {
            ResponseBase response = new ResponseBase();
            response.Message = "Add Picture";
            if (IdAlbum != null && IdAlbum != 0)
            {
                response.Message = response.Message + " Album ";

            }
            string sfolder = "";

            switch (type)
            {
                case 0:
                    sfolder = "hotel";

                    break;
                case 1:
                    sfolder = "room";

                    break;
                case 2:
                    sfolder = "tour";

                    break;
                default:
                    sfolder = "news";
                    break;
            }
            try
            {
                UploadImagesResponse temp = new UploadImagesResponse();


                //temp = _client.PostImgAndGetData(picture, 1024, Obj, userId, type);
                temp = imagesResponse;


                string[] charsToRemove = new string[] { "@", "[", "]", "'" };
                foreach (var c in charsToRemove)
                {
                    temp.data = temp.data.Replace(c, string.Empty);
                    temp.size = temp.size.Replace(c, string.Empty);
                }

                var url = temp.data.Split(",");
                var size = temp.size.Split(",");
                for (int i = 0; i < url.Length; i++)
                {

                    ResponseBase result = new ResponseBase();
                    Picture pic = new Picture();

                    switch (type)
                    {
                        case 0:

                            pic.HotelId = Obj;
                            break;
                        case 1:

                            pic.HotelRoomId = Obj;
                            break;
                        case 2:

                            pic.TourId = Obj;
                            break;
                        default:
                            sfolder = "news"; break;
                    }

                    pic.Type = type;
                    var path = Path.GetFileNameWithoutExtension(url[i]) + Path.GetExtension(url[i]).Replace("\"", "");

                    pic.Url = $"/partner/" + sfolder +$"/{userId}"+ "/" + Obj + "/" + path;
                    pic.IdType = Obj;
                    if (IdAlbum != null && IdAlbum != 0)
                    {
                        pic.IdAlbum = IdAlbum;
                    }
                    pic.Size = int.Parse(size[i]);
                    var message = AddNewPicture(pic);
                    if (message == "Add Picture Fail")
                    {
                        response.Message = response.Message + " Fail";

                    }
                    else
                    {
                        response.Message = response.Message + " Pass";
                    }

                }
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Add Picture Fail";
                response.Code = ErrorCodeMessage.AddFail.Key;
                return response;
            }
        }
        public string AddNewPicture(Picture picture)
        {
            try
            {

                _commonUoW.BeginTransaction();
                _pictureRepository.Insert(picture);
                _commonUoW.Commit();

                return "Add Picture Success";
            }
            catch
            {
                _commonUoW.RollBack();
                return "Add Picture Fail";
            }
        }
    }
}

