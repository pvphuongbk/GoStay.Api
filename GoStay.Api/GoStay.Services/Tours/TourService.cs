﻿using AutoMapper;
using GoStay.Common;
using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.HotelDto;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICommonRepository<TourProvinceTo> _tourProvinceToRepository;



        public TourService(ICommonRepository<Tour> tourRepository, IMapper mapper, ICommonRepository<TourStyle> tourStyleRepository,
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TourDetail> tourDetailRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<TinhThanh> provinceRepository, ICommonRepository<TourProvinceTo> tourProvinceToRepository)
        {
            _tourRepository = tourRepository;
            _mapper = mapper;
            _tourStyleRepository = tourStyleRepository;
            _tourTopicRepository = tourTopicRepository;
            _tourDetailRepository = tourDetailRepository;
            _pictureRepository = pictureRepository;
            _provinceRepository = provinceRepository;
            _tourProvinceToRepository = tourProvinceToRepository;
        }

        public ResponseBase SuggestTour(string searchText)
        {
            ResponseBase response = new ResponseBase();
            // Remove unicode
            searchText = searchText.RemoveUnicode();
            searchText = searchText.Replace(" ", string.Empty).ToLower();
            response.Data = TourRepository.SuggestTour(searchText);
            return response;
        }

        public ResponseBase SearchTour(SearchTourRequest request)
        {
            ResponseBase response = new ResponseBase();
            response.Data = TourRepository.GetPagingListTours(request);
            return response;
        }

        public ResponseBase GetTourContent(int Id)
        {
            
            ResponseBase response = new ResponseBase();
            try
            {
                var tour = _tourRepository.FindAll(x => x.Id == Id)
                                .Include(x=>x.OrderDetails)
                                .Include(x=>x.Pictures.Take(5))
                                .Include(x=>x.TourProvinceTos).SingleOrDefault();
                var tourContent = new TourContentDto();
                tourContent = _mapper.Map<Tour,TourContentDto >(tour);

                tourContent.TourStyle = _tourStyleRepository.GetById(tourContent.IdTourStyle).TourStyle1;

                tourContent.TourTopic = _tourTopicRepository.GetById(tourContent.IdTourTopic).TourTopic1;

                tourContent.ProvinceFrom = _provinceRepository.GetById(tourContent.IdProvinceFrom).TenTt;

                tourContent.IdProvinceTo = _tourProvinceToRepository.FindAll(x => x.IdTour == tourContent.Id).Select(x => x.IdProvinceTo).ToList();

                tourContent.ProvinceTo = new List<string>();
                foreach(var item in tourContent.IdProvinceTo)
                {
                    tourContent.ProvinceTo.Add(_provinceRepository.GetById(item).TenTt);
                }

                tourContent.Pictures = tour.Pictures.Select(x=>x.Url).ToList();

                tourContent.TourDetails = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(_tourDetailRepository.FindAll(x => x.IdTours == Id).ToList());

                response.Data = tourContent;
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
