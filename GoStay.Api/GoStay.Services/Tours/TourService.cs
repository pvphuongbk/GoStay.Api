using AutoMapper;
using GoStay.Common;
using GoStay.Common.Configuration;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.Enums;
using GoStay.Data.HotelDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.UnitOfWork;
using GoStay.DataDto;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Services.Tours
{
    public class TourService : ITourService
    {
        private readonly ICommonRepository<Tour> _tourRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<TourStyle> _tourStyleRepository;
        private readonly ICommonRepository<TourTopic> _tourTopicRepository;
        private readonly ICommonRepository<TourDetail> _tourDetailRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<TinhThanh> _provinceRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourLocationToRepository;
        private readonly ICommonRepository<Quan> _districtRepository;
        private readonly ICommonRepository<TourRating> _tourRatingRepository;
        private readonly ICommonRepository<TourStartTime> _tourStartTimeRepository;
        private readonly ICommonRepository<Vehicle> _vehicleRepository;
        private readonly ICommonRepository<TourVehicle> _tourVehicleRepository;
        private readonly ICommonRepository<CompareTour> _compareTourRepository;

        private readonly ICommonUoW _commonUoW;

        public TourService(ICommonRepository<Tour> tourRepository, IMapper mapper, ICommonRepository<TourStyle> tourStyleRepository,
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TourDetail> tourDetailRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<TinhThanh> provinceRepository, 
            ICommonRepository<TourDistrictTo> tourLocationToRepository, ICommonRepository<Quan> districtRepository,
            ICommonRepository<TourRating> tourRatingRepository, ICommonRepository<TourStartTime> tourStartTimeRepository,
            ICommonRepository<Vehicle> vehicleRepository, ICommonUoW commonUoW, ICommonRepository<TourVehicle> tourVehicleRepository,
            ICommonRepository<CompareTour> compareTourRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _tourStyleRepository = tourStyleRepository;
            _tourTopicRepository = tourTopicRepository;
            _tourDetailRepository = tourDetailRepository;
            _pictureRepository = pictureRepository;
            _provinceRepository = provinceRepository;
            _tourLocationToRepository = tourLocationToRepository;
            _districtRepository = districtRepository;
            _tourRatingRepository = tourRatingRepository;
            _tourStartTimeRepository = tourStartTimeRepository;
            _vehicleRepository = vehicleRepository;
            _commonUoW = commonUoW;
            _tourVehicleRepository = tourVehicleRepository;
            _compareTourRepository = compareTourRepository;
        }

        public ResponseBase SuggestTour(string searchText)
        {
            ResponseBase response = new ResponseBase();
            // Remove unicode
            searchText = searchText.RemoveUnicode();
            searchText = searchText.Replace(" ", string.Empty).ToLower();
            var Data = TourRepository.SuggestTour(searchText);

            Data.ForEach(x=>x.Slug = x.Name.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower());   
            response.Data=Data;
            return response;
        }

        public ResponseBase SearchTour(SearchTourRequest request)
        {
            ResponseBase response = new ResponseBase();
            var Data = TourRepository.GetPagingListTours(request);
            Data.ForEach(x => x.Slug = (x.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
            response.Data = Data;
            return response;
        }
        public ResponseBase GetTourHomePage()
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data = new List<SearchTourDto>();
                for (int i = 1; i <= 6; i++)
                {
                    SearchTourRequest request = new SearchTourRequest() { IdTourStyle = new int[] { i }, PageIndex = 1, PageSize = 4 };
                    var data = TourRepository.GetPagingListTours(request);

                    Data.AddRange(data);
                }
                Data.ForEach(x => x.Slug = (x.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
                response.Data = Data;
                return response;
            }
            catch
            {
                response.Data = new TourContentDto();
                return response;
            }

        }

        public ResponseBase GetAllTourByUserId(int UserId, int PageIndex, int PageSize)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data =new List<TourAdminDto>();
                var count = _tourRepository.FindAll(x => x.IdUser == UserId && x.Deleted != 1).Count();
                var tours = _tourRepository.FindAll(x => x.IdUser == UserId && x.Deleted != 1).OrderByDescending(x=>x.Id).Skip(PageSize * (PageIndex - 1)).Take(PageSize).ToList();
                Data = _mapper.Map<List<Tour>,List<TourAdminDto>>(tours);
                response.Data = Data;
                response.Count = count;
                return response;
            }
            catch
            {
                response.Data = new List<TourAdminDto>();
                return response;
            }

        }


        public ResponseBase GetTourByUserIdAndId(int UserId, int Id)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data = new TourAdminDto();

                var tours = _tourRepository.FindAll(x => x.IdUser == UserId && x.Deleted != 1&&x.Id==Id).SingleOrDefault();
                Data = _mapper.Map<Tour, TourAdminDto>(tours);
                Data.IdDistrictTo = _tourLocationToRepository.FindAll(x => x.IdTour == Id).Select(x => x.IdDistrictTo).ToArray();
                Data.Vehicles = _tourVehicleRepository.FindAll(x => x.IdTour == Id).Select(x => x.IdVehicle).ToArray();

                response.Data = Data;
                return response;
            }
            catch
            {
                response.Data = new TourAdminDto();
                return response;
            }

        }

        public ResponseBase GetTourContent(int Id)
        {
            
            ResponseBase response = new ResponseBase();
            try
            {
                var tour = _tourRepository.FindAll(x => x.Id == Id)
                                .Include(x=>x.TourDetails)
                                .Include(x=>x.IdTourStyleNavigation)
                                .Include(x => x.IdTourTopicNavigation)
                                .Include(x => x.IdDistrictFromNavigation).ThenInclude(x=>x.IdTinhThanhNavigation)
                                .Include(x=>x.OrderDetails)
                                .Include(x=>x.Pictures)
                                .Include(x=>x.TourDistrictTos).ThenInclude(x=>x.IdDistrictToNavigation).ThenInclude(x=>x.IdTinhThanhNavigation)
                                .Include(x=>x.IdStartTimeNavigation).SingleOrDefault();
                var tourContent = new TourContentDto();
                tourContent = _mapper.Map<Tour,TourContentDto >(tour);

                tourContent.TourStyle = tour.IdTourStyleNavigation.TourStyle1;

                tourContent.TourTopic = tour.IdTourTopicNavigation.TourTopic1;

                if(tour.IdDistrictFromNavigation.Tenquan == "Tất cả")
                {
                    tourContent.DistrictFrom = tour.IdDistrictFromNavigation.IdTinhThanhNavigation.TenTt;
                }
                else
                {
                    tourContent.DistrictFrom = tour.IdDistrictFromNavigation.Tenquan;
                }

                if (tour.IdStartTime != null)
                {
                    tourContent.StartTime = tour.IdStartTimeNavigation.StartDate;
                }
                tourContent.IdDistrictTo = new Dictionary<int, string>();
                foreach (var item in tour.TourDistrictTos)
                {
                    if (item.IdDistrictToNavigation.Tenquan == "Tất cả")
                    {
                        tourContent.IdDistrictTo.Add(item.IdDistrictTo, item.IdDistrictToNavigation.IdTinhThanhNavigation.TenTt);
                    }
                    else
                    {
                        tourContent.IdDistrictTo.Add(item.IdDistrictTo, item.IdDistrictToNavigation.Tenquan);
                    }
                }
                tourContent.Pictures = tour.Pictures.Where(x=>x.Deleted!=1).Select(x=>x.Url).ToList();

                tourContent.TourDetails = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(tour.TourDetails.Where(x=>x.Deleted!=1).OrderBy(x => x.Stt).ToList());

                tourContent.Slug= tourContent.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower();
                response.Data = tourContent;
                return response;
            }
            catch
            {
                response.Data = new TourContentDto();
                return response;
            }

        }
        public ResponseBase GetTourLocationTotal(int IdProvince)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var dicstrictTo = _tourLocationToRepository.FindAll(x => x.IdDistrictToNavigation.IdTinhThanhNavigation != null &&
                                                                    x.IdDistrictToNavigation.IdTinhThanhNavigation.Id == IdProvince).Count();
                response.Data = dicstrictTo;
                return response;
            }
            catch
            {
                response.Data = 0;
                return response;
            }

        }
        public ResponseBase GetDataSupportTour()
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data = new DataSupportTour();
                var topics = _tourTopicRepository.FindAll().OrderBy(x => x.TourTopic1).ToList();
                Data.Topics = _mapper.Map<List<TourTopic>, List<TourTopicDto>>(topics);

                var ratings =_tourRatingRepository.FindAll().ToList();
                Data.Ratings = _mapper.Map<List<TourRating>, List<TourRatingDto>>(ratings);

                var starttimes = _tourStartTimeRepository.FindAll().ToList();
                Data.StartTimes = _mapper.Map<List<TourStartTime>, List<TourStartTimeDto>>(starttimes);

                var quans = _districtRepository.FindAll().Include(x => x.IdTinhThanhNavigation).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).ToList();
                    Data.Districts = _mapper.Map<List<Quan>, List<QuanDto>>(quans);
                Data.Districts.ForEach(x => x.ProvinceName = quans.Where(y => y.Id == x.Id).Single().IdTinhThanhNavigation.TenTt);

                var styles = _tourStyleRepository.FindAll().ToList();
                Data.Styles = _mapper.Map<List<TourStyle>, List<TourStyleDto>>(styles);

                var provinces = _provinceRepository.FindAll().OrderBy(x => x.TenTt).OrderBy(x => x.Stt).ToList();
                Data.Provinces = _mapper.Map<List<TinhThanh>, List<ProvinceDto>>(provinces);

                var vehicles = _vehicleRepository.FindAll().ToList();
                Data.Vehicles = _mapper.Map<List<Vehicle>, List<VehicleDto>>(vehicles);

                response.Data = Data;
                return response;
            }
            catch
            {
                response.Data = null;
                return response;
            }

        }
        public ResponseBase AddTour(Tour data, int[] IdDistrictTo, int[] Vehicles)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                data.InDate = (int)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;
                _commonUoW.BeginTransaction();
                data.PriceChild = data.ActualPrice * 7 / 10;
                _tourRepository.Insert(data);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                foreach (var item in IdDistrictTo)
                {
                    _tourLocationToRepository.Insert(new TourDistrictTo() { IdTour = data.Id, IdDistrictTo = item });
                }
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();
                foreach (var item in Vehicles)
                {
                    _tourVehicleRepository.Insert(new TourVehicle() { IdTour = data.Id, IdVehicle = (byte)item });
                }
                _commonUoW.Commit();
                response.Data = data.Id;
                return response;
            }
            catch(Exception e)
            {
                _commonUoW.RollBack();
                response.Data = 0;
                response.Message = e.Message;
                return response;

            }
        }
        public ResponseBase AddTourDetail(TourDetail data)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                _tourDetailRepository.Insert(data);
                _commonUoW.Commit();
                response.Data = data.Id;
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Data = 0;
                return response;
            }
        }
        public ResponseBase EditTour(TourAddDto data)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();

                var tour = _tourRepository.FindAll(x => x.Id == data.Id).Take(1).AsNoTracking().SingleOrDefault();
                if (data.StartDateString != "" && data.StartDateString != null)
                {
                    data.StartDateString = data.StartDateString.Trim();
                    try
                    {
                        tour.StartDate = DateTime.ParseExact(data.StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        _commonUoW.Commit();
                        data.IdStartTime = AddTourStartTime(data.StartDateString);
                        _commonUoW.BeginTransaction();

                    }
                }
                else
                {
                    tour.StartDate = new DateTime(2100, 1, 1);
                }
                tour.TourName = data.TourName;
                tour.IdUser = data.IdUser;
                tour.IdTourStyle = data.IdTourStyle;
                tour.IdTourTopic = data.IdTourTopic;
                tour.IdDistrictFrom = data.IdDistrictFrom;
                tour.Price = data.Price;
                tour.Discount = data.Discount;
                tour.Content = data.Content;
                tour.TourSize = data.TourSize;
                tour.Locations = data.Locations;
                tour.Style = data.Style;
                tour.Rating = data.Rating;
                tour.IdStartTime = data.IdStartTime;

                tour.Descriptions = data.Descriptions;
                tour.SearchKey = tour.TourName.RemoveUnicode();
                tour.SearchKey = tour.SearchKey.Replace(" ", string.Empty).ToLower();
                if (tour.Discount is null)
                    tour.Discount = 0;
                tour.Status = 1;
                tour.ActualPrice = tour.Price * (100 - (double)tour.Discount) / 100;
                tour.InDate = (int)(System.DateTime.Now - AppConfigs.startDate).TotalSeconds;

                _tourRepository.Update(tour);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                var listdistrictold = _tourLocationToRepository.FindAll(x => x.IdTour == data.Id).ToList();
                _tourLocationToRepository.RemoveMultiple(listdistrictold);
                foreach (var item in data.IdDistrictTo)
                {
                    _tourLocationToRepository.Insert(new TourDistrictTo() { IdTour = data.Id, IdDistrictTo = item });
                }
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();
                var listvehicleold = _tourVehicleRepository.FindAll(x => x.IdTour == data.Id).ToList();
                _tourVehicleRepository.RemoveMultiple(listvehicleold);
                foreach (var item in data.Vehicle)
                {
                    _tourVehicleRepository.Insert(new TourVehicle() { IdTour = data.Id, IdVehicle = (byte)item });
                }

                _commonUoW.Commit();

                response.Data = "Success";
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Data = "Exception";
                return response;
            }
        }
        public ResponseBase EditTourDetail(TourDetail data)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                _tourDetailRepository.Update(data);
                _commonUoW.Commit();
                response.Data = "Success";
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Data = "Exception";
                return response;
            }
        }

        public ResponseBase DeleteTour(int id)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var tour = _tourRepository.GetById(id);
                tour.Deleted = 1;
                _tourRepository.Update(tour);
                _commonUoW.Commit();
                response.Data = "Success";
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Data = "Exception";
                return response;
            }
        }

        public ResponseBase RemoveTourDetail(int IdDetail)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var tourdetailentity = _tourDetailRepository.GetById(IdDetail);
                if (tourdetailentity != null)
                {
                    tourdetailentity.Deleted = 1;
                    _tourDetailRepository.Update(tourdetailentity);
                    _commonUoW.Commit();
                    response.Data = "Not found";
                    return response;

                }
                _commonUoW.Commit();
                response.Data = "Success";
                return response;
            }
            catch
            {
                _commonUoW.RollBack();
                response.Data = "Exception";
                return response;
            }
        }
        public int AddTourStartTime(string time)
        {
            try
            {
                var temp = _tourStartTimeRepository.FindAll(x => x.StartDate == time);
                if (temp.Count() == 0)
                {
                    TourStartTime data = new TourStartTime() { StartDate = time };
                    _commonUoW.BeginTransaction();

                    _tourStartTimeRepository.Insert(data);
                    _commonUoW.Commit();
                    return data.Id;
                }
                else
                {
                    return temp.First().Id;
                }
            }
            catch
            {
                _commonUoW.RollBack();
                return 0;
            }
        }

        public ResponseBase UpdateTourToCompare(CompareTourParam param)
        {
            ResponseBase response = new ResponseBase();

            try
            {
                _commonUoW.BeginTransaction();

                var comparetour = _compareTourRepository.FindAll(x=>x.IdUser==param.IdUser 
                                    && x.Session == param.Session && x.Deleted==false).SingleOrDefault();
                if (comparetour==null)
                {
                    var entity = new CompareTour { IdTours = param.IdTour + ",", Session = param.Session, IdUser = param.IdUser, Deleted = false };
                    _compareTourRepository.Insert(entity);
                    _commonUoW.Commit();
                    response.Data = entity.IdTours;
                    return response;
                }
                else
                {
                    if(comparetour.IdTours.Contains(param.IdTour) == true)
                    {
                        _commonUoW.Commit();
                        response.Data = comparetour.IdTours;
                        return response;
                    }
                    var temp = comparetour.IdTours.Remove(comparetour.IdTours.Length - 1);
                    var stringIdA = temp.Split(",");

                    var listId = Array.ConvertAll<string, int>(stringIdA, int.Parse).ToList();
                    if (listId.Count() == 10)
                    {

                        listId.RemoveAt(0);
                        comparetour.IdTours = "";
                        foreach (var item in listId)
                        {
                            comparetour.IdTours += $"{item},";
                        }
                    }    
                    comparetour.IdTours = comparetour.IdTours+ $"{param.IdTour},";
                    _compareTourRepository.Update(comparetour);
                    _commonUoW.Commit();
                    response.Data = comparetour.IdTours;
                    return response;
                }    
            }
            catch(Exception ex)
            {
                _commonUoW.RollBack();
                response.Data = ex.Message;
                return response;
            }
        }

        public ResponseBase GetListToursCompare( string ListId)
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data = TourRepository.GetListToursCompare(ListId);
                Data.ForEach(x => x.Slug = (x.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                                            .Replace("/", "-").Replace("--", string.Empty).Replace(".", "-")
                                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                                            .Replace("(", string.Empty).Replace(")", string.Empty)
                                            .Replace("*", string.Empty).Replace("%", string.Empty)
                                            .Replace("&", "-").Replace("@", string.Empty).ToLower()));
                response.Data = Data;
                return response;
            }
            catch
            {
                response.Data = new TourContentDto();
                return response;
            }

        }
    }
}
