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
            foreach (var Tour in Data)
            {
                Tour.Slug = Tour.Name.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty).Replace("--", string.Empty).ToLower();
                if(Tour.Type== SuggestTourType.TourName)
                {
                    Tour.Img = _pictureRepository.FindAll(x => x.TourId == Tour.Id).FirstOrDefault().UrlOut;
                }    
            }

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
                                .Include(x=>x.OrderDetails)
                                .Include(x=>x.Pictures)
                                .Include(x=>x.TourDistrictTos)
                                .Include(x=>x.IdStartTimeNavigation).SingleOrDefault();
                var tourContent = new TourContentDto();
                tourContent = _mapper.Map<Tour,TourContentDto >(tour);

                tourContent.TourStyle = _tourStyleRepository.GetById(tourContent.IdTourStyle).TourStyle1;

                tourContent.TourTopic = _tourTopicRepository.GetById(tourContent.IdTourTopic).TourTopic1;

                tourContent.DistrictFrom = _districtRepository.GetById(tourContent.IdDistrictFrom).Tenquan;

                tourContent.IdDistrictTo = _tourLocationToRepository.FindAll(x => x.IdTour == tourContent.Id).Select(x => x.IdDistrictTo).ToList();
                if (tour.IdStartTime != null)
                {
                    tourContent.StartTime = tour.IdStartTimeNavigation.StartDate;
                }
                tourContent.DistrictTo = new List<string>();
                foreach(var item in tourContent.IdDistrictTo)
                {
                    tourContent.DistrictTo.Add(_districtRepository.GetById(item).Tenquan);
                }

                tourContent.Pictures = tour.Pictures.Where(x=>x.Deleted!=1).Select(x=>x.Url).ToList();

                tourContent.TourDetails = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(_tourDetailRepository.FindAll(x => x.IdTours == Id&& x.Deleted!=1).OrderBy(x=>x.Stt).ToList());
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

                var listdistrict = _districtRepository.FindAll(x => x.IdTinhThanh == IdProvince).Select(x=>x.Id);
                int total=0;
                foreach(var item in listdistrict)
                {
                    total += _tourLocationToRepository.FindAll(x => x.IdDistrictTo == item).Count();
                }    
                response.Data = total;

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
