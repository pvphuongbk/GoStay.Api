using AutoMapper;
using GoStay.Common;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.Enums;
using GoStay.Data.HotelDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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



        public TourService(ICommonRepository<Tour> tourRepository, IMapper mapper, ICommonRepository<TourStyle> tourStyleRepository,
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TourDetail> tourDetailRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<TinhThanh> provinceRepository, 
            ICommonRepository<TourDistrictTo> tourLocationToRepository, ICommonRepository<Quan> districtRepository)
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
        }

        public ResponseBase SuggestTour(string searchText)
        {
            ResponseBase response = new ResponseBase();
            // Remove unicode
            searchText = searchText.RemoveUnicode();
            searchText = searchText.Replace(" ", string.Empty).ToLower();
            var Data = TourRepository.SuggestTour(searchText);

            Data.ForEach(x=>x.Slug = x.Name.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower());   
            response.Data=Data;
            return response;
        }

        public ResponseBase SearchTour(SearchTourRequest request)
        {
            ResponseBase response = new ResponseBase();
            var Data = TourRepository.GetPagingListTours(request);
            Data.ForEach(x => x.Slug = (x.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()));
            response.Data = Data;
            return response;
        }
        public ResponseBase GetTourHomePage()
        {

            ResponseBase response = new ResponseBase();
            try
            {
                var Data =new List<SearchTourDto>();
                for (int i=1;i<=6;i++)
                {
                    SearchTourRequest request = new SearchTourRequest() { IdTourStyle = new int[] {i}, PageIndex = 1, PageSize = 4 };
                    var data = TourRepository.GetPagingListTours(request);
                    
                    Data.AddRange(data);
                }
                Data.ForEach(x => x.Slug = (x.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower()));
                response.Data = Data;
                return response;
            }
            catch
            {
                response.Data = new TourContentDto();
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

                tourContent.Slug= tourContent.TourName.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower();
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
    }
}
