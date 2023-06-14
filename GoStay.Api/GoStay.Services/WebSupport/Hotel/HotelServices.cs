
using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Hotel;
using GoStay.DataDto.HotelDto;
using GoStay.Repository.Repositories;
using GoStay.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
            if (request.NameSearch == null)
            {
                request.NameSearch = "";
            }
            request.NameSearch = request.NameSearch.RemoveUnicode();
            request.NameSearch = request.NameSearch.Replace(" ", string.Empty).ToLower();
            PagingList<Hotel> hotel = new PagingList<Hotel>();
            if (request.IdProvince == null || request.IdProvince == 0)
            {
                hotel = _hotelRepository.FindAll(x => x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x => x.IdPriceRangeNavigation)
                    .Include(x => x.TypeNavigation)
                    .Include(x=>x.HotelRooms.Where(z=>z.Iduser== request.UserId && z.Deleted !=1))
                    .OrderByDescending(x => x.Id)
                    .ConvertToPaging(request.PageSize ?? 10, request.PageIndex ?? 1);
            }
            else
            {
                hotel = _hotelRepository.FindAll(x => x.IdTinhThanh == request.IdProvince && x.Deleted != 1 && x.SearchKey.Contains(request.NameSearch) == true)
                    .Include(x => x.IdPriceRangeNavigation)
                    .Include(x => x.TypeNavigation)
                    .Include(x => x.HotelRooms.Where(z => z.Iduser == request.UserId && z.Deleted != 1))
                    .OrderByDescending(x=>x.Id)
                    .ConvertToPaging(request.PageSize ?? 10, request.PageIndex ?? 1);
            }
            var list = _mapper.Map<PagingList<Hotel>, PagingList<HotelDto>>(hotel);
            list.Items.ForEach(x => x.PriceRange = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().IdPriceRangeNavigation.TitleVnd));
            list.Items.ForEach(x => x.TypeHotel = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().TypeNavigation.Type));
            list.Items.ForEach(x => x.RoomCount = (hotel.Items.Where(y => y.Id == x.Id).FirstOrDefault().HotelRooms
                                                    .Count()));


            response.Data = list;
            return response;
        }

        public ResponseBase GetHotelListByUser(int IdUser)
        {
            ResponseBase response = new ResponseBase();
            List<HotelListUserDto> list = new List<HotelListUserDto>();

            var hotels = _hotelRepository.FindAll(x => x.HotelRooms.Any(x => x.Iduser == IdUser))
                            .Include(x=>x.HotelRooms.Where(r=>r.Iduser== IdUser && r.Deleted != 1))
                            .OrderByDescending(x => x.Id);
            if (hotels != null)
            {
                foreach (var hotel in hotels)
                {
                    list.Add(new HotelListUserDto { Id = hotel.Id, Name = hotel.Name,RoomCount = hotel.HotelRooms.Count() });
                }
            }
            response.Data = list;
            return response;
        }
        public ResponseBase GetRoomList(RequestGetListRoom request)
        {
            ResponseBase response = new ResponseBase();

            try
            {

                PagingList<HotelRoom> rooms = new PagingList<HotelRoom>();

                if (request.IdRoom != null && request.IdRoom != 0)
                {
                    rooms = _roomRepository.FindAll(x => x.Deleted != 1 && x.Iduser == request.IdUser && x.Id == request.IdRoom)
                        .Include(x => x.PalletbedNavigation)
                        .Include(x => x.RoomViews).ThenInclude(x => x.IdViewNavigation)
                        .Include(x => x.RoomMamenitis).ThenInclude(x => x.IdservicesNavigation)
                        .Include(x => x.IdhotelNavigation)
                        .Include(x => x.Pictures)
                        .OrderByDescending(x=>x.Id)
                        .ConvertToPaging(1, 1);
                }
                else
                {
                    if (request.NameSearch == null)
                    {
                        request.NameSearch = "";
                    }
                    request.NameSearch = request.NameSearch.RemoveUnicode();
                    request.NameSearch = request.NameSearch.Replace(" ", string.Empty).ToLower();

                    if (request.IdHotel == null || request.IdHotel == 0)
                    {
                        rooms = _roomRepository.FindAll(x => x.Deleted != 1 && x.Iduser == request.IdUser && x.SearchKey.Contains(request.NameSearch) == true)
                            .Include(x => x.PalletbedNavigation)
                            .Include(x => x.RoomViews).ThenInclude(x => x.IdViewNavigation)
                            .Include(x => x.RoomMamenitis).ThenInclude(x => x.IdservicesNavigation)
                            .Include(x => x.IdhotelNavigation)
                            .Include(x => x.Pictures)
                            .OrderByDescending(x => x.Id)
                            .ConvertToPaging(request.PageSize ?? 10, request.PageIndex ?? 1);
                    }
                    else
                    {
                        rooms = _roomRepository.FindAll(x => x.Deleted != 1 && x.Iduser == request.IdUser && x.Idhotel == request.IdHotel
                                                        && x.SearchKey.Contains(request.NameSearch) == true)
                            .Include(x => x.PalletbedNavigation)
                            .Include(x => x.RoomViews).ThenInclude(x => x.IdViewNavigation)
                            .Include(x => x.RoomMamenitis).ThenInclude(x => x.IdservicesNavigation)
                            .Include(x => x.IdhotelNavigation)
                            .Include(x => x.Pictures)
                            .OrderByDescending(x => x.Id)
                            .ConvertToPaging(request.PageSize ?? 10, request.PageIndex ?? 1);
                    }
                }

                var list = _mapper.Map<PagingList<HotelRoom>, PagingList<RoomDto>>(rooms);

                list.Items.ForEach(x => x.PalletbedText = (rooms.Items.FindAll(z => z.Id == x.Id).FirstOrDefault().PalletbedNavigation.Text));
                list.Items.ForEach(x => x.HotelName = (rooms.Items.FindAll(z => z.Id == x.Id).FirstOrDefault().IdhotelNavigation.Name));

                foreach (var room in rooms.Items)
                {
                    list.Items.Where(x => x.Id == room.Id).FirstOrDefault().ViewsRoom =
                        _mapper.Map<List<ViewDirection>, List<ViewRoomDto>>(room.RoomViews.Select(x => x.IdViewNavigation).ToList());
                    list.Items.Where(x => x.Id == room.Id).FirstOrDefault().ServicesRoom =
                        _mapper.Map<List<Service>, List<ServiceRoomDto>>(room.RoomMamenitis.Select(x => x.IdservicesNavigation).ToList());
                    list.Items.Where(x => x.Id == room.Id).FirstOrDefault().PicturesRoom =
                        _mapper.Map<List<Picture>, List<PictureRoomDto>>(room.Pictures.ToList());
                }

                response.Data = list;
                return response;
            }
            catch
            {
                response.Data = new PagingList<RoomDto>();
                return response;
            }
        }
        public ResponseBase GetListRoomAdmin(RequestGetListRoomAdmin request)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                if (request.HotelName == null)
                {
                    request.HotelName = "";
                }
                if (request.RoomName == null)
                {
                    request.RoomName = "";
                }
                request.HotelName = request.HotelName.RemoveUnicode();
                request.HotelName = request.HotelName.Replace(" ", string.Empty).ToLower();
                request.RoomName = request.RoomName.RemoveUnicode();
                request.RoomName = request.RoomName.Replace(" ", string.Empty).ToLower();
                var data = HotelRepository.GetListRoomAdmin(request);

                response.Data = data;
                return response;
            }
            catch
            {
                response.Data = new PagingList<RoomDto>();
                return response;
            }
        }
        public ResponseBase AddRoom(HotelRoom data)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                if (data.Iduser == 9)
                {
                    data.Status = 1;//đc duyệt
                }
                else
                {
                    data.Status = 0;//chờ duyệt
                }
                data.SearchKey = data.Name.RemoveUnicode().Replace(" ", string.Empty).ToLower();
                data.RoomStatus = 0;
                _commonUoW.BeginTransaction();
                _roomRepository.Insert(data);
                _commonUoW.Commit();

                response.Message = "Add Room Success";
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
        public ResponseBase EditRoom(HotelRoom data, List<int> view, List<int> service, List<int> picture)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                if (data.Iduser == 9)
                {
                    data.Status = 1;//đc duyệt
                }
                else
                {
                    data.Status = 0;//chờ duyệt
                }
                data.IntDate = (long)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;
                _commonUoW.BeginTransaction();
                var listviewold = _roomViewsRepository.FindAll(x => x.IdRoom == data.Id).ToList();
                if (listviewold != null)
                {
                    _roomViewsRepository.RemoveMultiple(listviewold);
                }
                if (view != null)
                {
                    foreach (var item in view)
                    {
                        _roomViewsRepository.Insert(new RoomView() { IdRoom = data.Id, IdView = item });
                    }
                }
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                var listserviceold = _roomServicesRepository.FindAll(x => x.Idroom == data.Id).ToList();
                if (listserviceold != null)
                {
                    _roomServicesRepository.RemoveMultiple(listserviceold);
                }
                if (service != null)
                {
                    foreach (var item in service)
                    {
                        _roomServicesRepository.Insert(new RoomMameniti() { Idroom = data.Id, Idservices = item });
                    }
                }
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();

                if (picture != null)
                {
                    foreach (var item in picture)
                    {
                        DeletePicture(item);
                    }
                }
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                var hotel = _hotelRepository.GetById(data.Idhotel);
                hotel.IntDate = data.IntDate;
                var temp = _roomRepository.FindAll().Where(x => x.Id == data.Id).Take(1).AsNoTracking();
                data.CreatedDate = temp.FirstOrDefault().CreatedDate;
                data.RemainNum = temp.FirstOrDefault().RemainNum;
                _roomRepository.Update(data);
                _hotelRepository.Update(hotel);
                _commonUoW.Commit();
                response.Message = "Edit Room Success";
                response.Code = ErrorCodeMessage.Success.Key;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Edit Room Fail";
                response.Code = ErrorCodeMessage.EditFail.Key;
                return response;
            }
        }
        public ResponseBase SupportAddRoom()
        {
            ResponseBase response = new ResponseBase();
            SupportAddRoom support = new SupportAddRoom();
            support.views = _viewRepository.FindAll().ToList();
            support.palletbed = _palletbedRepository.FindAll().ToList();
            support.servicesRoom = _servicesRepository.FindAll(x => x.IdStyle == 1).ToList();

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
                if (checkName != null && checkName.Count() < 1)
                {
                    var data = new Album() { Name = albumName, IdRoom = IdRoom, TypeAlbum = 1 };
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
        public ResponseBase AddPictureRoom(int Obj, int IdAlbum, int type, int userId, UploadImagesResponse imagesResponse)
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

                    pic.Url = $"/partner/" + sfolder + $"/{userId}" + "/" + Obj + "/" + path;
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
        public ResponseBase UpdateRoomStatus(UpdateStatusRoomParam param)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var room = _roomRepository.FindAll(x => x.Iduser == param.UserId && x.Id == param.RoomId).SingleOrDefault();
                if (room != null)
                {
                    room.RoomStatus = param.Status;
                    _roomRepository.Update(room);
                    _commonUoW.Commit();
                    response.Message = "Update Status Success";
                    response.Code = ErrorCodeMessage.Success.Key;
                    return response;
                }
                else
                {
                    _commonUoW.Commit();
                    response.Message = "Valid Error";
                    response.Code = ErrorCodeMessage.NoObject.Key;
                    return response;
                }
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                return response;
            }

        }
        public ResponseBase UpdateRoomDiscount(UpdateDiscountRoomParam param)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var room = _roomRepository.FindAll(x => x.Iduser == param.UserId && x.Id == param.RoomId).SingleOrDefault();
                if (room != null)
                {
                    room.Discount = param.Discount;
                    room.NewPrice = (decimal)room.CurrentPrice * (decimal)(100 - room.Discount) / 100;
                    _roomRepository.Update(room);
                    _commonUoW.Commit();
                    response.Message = "Update Discount Success";
                    response.Code = ErrorCodeMessage.Success.Key;
                    return response;
                }
                else
                {
                    _commonUoW.Commit();
                    response.Message = "Valid Error";
                    response.Code = ErrorCodeMessage.NoObject.Key;
                    return response;
                }
            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
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
        public string DeletePicture(int Id)
        {
            try
            {
                _pictureRepository.Remove(Id);
                return "Success";
            }
            catch
            {
                return "Exception, Fail";
            }
        }

        public ResponseBase GetPicturesRoom(int IdRoom)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                List<PictureRoomDto> list = new List<PictureRoomDto>();

                var pictures = _pictureRepository.FindAll(x => x.HotelRoomId == IdRoom && x.Deleted != 1).OrderByDescending(x => x.Size);
                if (pictures != null)
                {
                    foreach (var picture in pictures)
                    {
                        list.Add(new PictureRoomDto { Id = picture.Id, UrlOut = picture.UrlOut });
                    }
                }
                response.Message = "Success";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = list;
                return response;
            }
            catch
            {
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                return response;

            }
        }
        public ResponseBase GetPicturesHotel(int IdHotel)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                List<PictureRoomDto> list = new List<PictureRoomDto>();

                var pictures = _pictureRepository.FindAll(x => x.HotelId == IdHotel && x.Deleted != 1).OrderByDescending(x => x.Size);
                if (pictures != null)
                {
                    foreach (var picture in pictures)
                    {
                        list.Add(new PictureRoomDto { Id = picture.Id, UrlOut = picture.UrlOut });
                    }
                }
                response.Message = "Success";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = list;
                return response;
            }
            catch
            {
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                return response;

            }
        }

        public ResponseBase GetServicesRoom(int IdRoom)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                List<ServiceRoomDto> list = new List<ServiceRoomDto>();

                var services = _roomServicesRepository.FindAll(x => x.Idroom == IdRoom).Select(x => x.IdservicesNavigation);
                if (services != null)
                {
                    foreach (var service in services)
                    {
                        list.Add(new ServiceRoomDto { Id = service.Id, Name = service.Name,AdvantageLevel= service.AdvantageLevel,
                                    Icon=service.Icon, NameChi= service.NameChi, NameEng = service.NameEng
                        });
                    }
                }
                response.Message = "Success";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = list;
                return response;
            }
            catch
            {
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                return response;

            }
        }
        public ResponseBase SetMapHotel(SetMapHotelRequest param)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var hotel = _hotelRepository.FindAll(x => x.Id == param.HotelId).SingleOrDefault();
                if (hotel == null )
                {
                    _commonUoW.Commit();
                    response.Message = "Update Map Fail";
                    response.Code = ErrorCodeMessage.InternalExeption.Key;
                    response.Data = "fail";
                    return response;
                }
                hotel.LonMap = param.LON;
                hotel.LatMap = param.LAT;
                _hotelRepository.Update(hotel);

                _commonUoW.Commit();
                response.Message = "Update Success";
                response.Code = ErrorCodeMessage.Success.Key;
                response.Data = "success";

                return response;

            }
            catch
            {
                _commonUoW.RollBack();
                response.Message = "Exception";
                response.Code = ErrorCodeMessage.InternalExeption.Key;
                response.Data = "fail";
                return response;
            }

        }
        public ResponseBase ChangeRoomStatus(int IdRoom, int Status)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                _commonUoW.BeginTransaction();
                var room = _roomRepository.FindAll(x=>x.Id==IdRoom).SingleOrDefault();
                room.Status = Status;
                _roomRepository.Update(room);
                _commonUoW.Commit();
                response.Data = "success";
                return response;

            }
            catch
            {
                response.Data = "exception";
                return response;
            }
        }
        public ResponseBase ChangeStatusRoom(int IdRoom, int RoomStatus)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                _commonUoW.BeginTransaction();
                var room = _roomRepository.FindAll(x => x.Id == IdRoom).SingleOrDefault();
                room.RoomStatus = RoomStatus;
                _roomRepository.Update(room);
                _commonUoW.Commit();
                response.Data = "success";
                return response;

            }
            catch
            {
                response.Data = "exception";
                return response;
            }
        }
    }
}

