﻿using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.RatingDto;
using GoStay.Repository.Repositories;
using GoStay.Services.Ratings;

namespace GoStay.Services.Reviews
{
    public class RatingService : IRatingService
    {
        private readonly ICommonRepository<HotelRating> _hotelRatingRepository;
        private readonly ICommonUoW _icommonUoWRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;

        public RatingService(ICommonRepository<HotelRating> hotelRatingRepository, ICommonUoW icommonUoWRepository, ICommonRepository<Hotel> hotelRepository)
        {
            _hotelRatingRepository = hotelRatingRepository;
            _icommonUoWRepository = icommonUoWRepository;
            _hotelRepository = hotelRepository;
        }
        
        public ResponseBase GetRatingByUser(int hotelId, int userId)
        {
            ResponseBase response = new ResponseBase();
            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == userId && x.IdHotel == hotelId).FirstOrDefault();
            if (exitsRating != null)
            {
                var dto = new GetRatingDto
                {
                    LocationScore = exitsRating.LocationScore,
                    ValueScore = exitsRating.ValueScore,
                    ServiceScore = exitsRating.ServiceScore,
                    CleanlinessScore = exitsRating.CleanlinessScore,
                    RoomsScore = exitsRating.RoomsScore,
                    Description = exitsRating.Description,
                };

                response.Data = dto;
            }

            return response;
        }
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto)
        {
            ResponseBase response = new ResponseBase();
            
            _icommonUoWRepository.BeginTransaction();
            var (re, check) = UpdateScoreForHotel(dto);
            if (!check)
            {
                response.Message = "User Id không tồn tại";
                return response;
            }

            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == dto.UserId && x.IdHotel == dto.HotelId).FirstOrDefault();
            if(exitsRating != null)
            {
                exitsRating.LocationScore = dto.LocationScore;
                exitsRating.ValueScore = dto.ValueScore;
                exitsRating.ServiceScore = dto.ServiceScore;
                exitsRating.CleanlinessScore = dto.CleanlinessScore;
                exitsRating.RoomsScore = dto.RoomsScore;
                exitsRating.Description = dto.Description;
                exitsRating.DateUpdate = DateTime.Now;
                _hotelRatingRepository.Update(exitsRating);
            }
            else
            {
                var item = new HotelRating
                {
                    IdHotel = dto.HotelId,
                    IdUser = dto.UserId,
                    Description = dto.Description,
                    ServiceScore = dto.ServiceScore,
                    CleanlinessScore = dto.CleanlinessScore,
                    LocationScore = dto.LocationScore,
                    RoomsScore = dto.RoomsScore,
                    ValueScore = dto.ValueScore,
                    DateReviews = DateTime.Now,   
                };
                _hotelRatingRepository.Insert(item);
            }

            _icommonUoWRepository.Commit();
            response.Data = re;
            return response;
        }

        private (UpdateRatingResponse,bool) UpdateScoreForHotel(RatingOrUpdateDto dto)
        {
            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser != dto.UserId && x.IdHotel == dto.HotelId).ToList();
            var hotel = _hotelRepository.GetById(dto.HotelId);
            if (hotel == null)
                return (null,false);
            var count = exitsRating.Count() + 1;
            UpdateLocationScore(hotel, count, exitsRating, dto);
            UpdateValueScore(hotel, count, exitsRating, dto);
            UpdateServiceScore(hotel, count, exitsRating, dto);
            UpdateCleanlinessScore(hotel, count, exitsRating, dto);
            UpdateCleanRoomsScore(hotel, count, exitsRating, dto);
            UpdateReview_score(hotel);
            UpdateRatingResponse re = new UpdateRatingResponse();
            re.LocationScore = hotel.LocationScore;
            re.ValueScore = hotel.ValueScore;
            re.ServiceScore = hotel.ServiceScore;
            re.CleanlinessScore = hotel.CleanlinessScore;
            re.RoomsScore = hotel.RoomsScore;
            re.ReviewScore = hotel.ReviewScore;

            return (re,true);
        }

        private void UpdateLocationScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.LocationScore) + dto.LocationScore;
            hotel.LocationScore = totalScore / count;
        }
        private void UpdateValueScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.ValueScore) + dto.ValueScore;
            hotel.ValueScore = totalScore / count;
        }
        private void UpdateServiceScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.ServiceScore) + dto.ServiceScore;
            hotel.ServiceScore = totalScore / count;
        }
        private void UpdateCleanlinessScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.CleanlinessScore) + dto.CleanlinessScore;
            hotel.CleanlinessScore = totalScore / count;
        }
        private void UpdateCleanRoomsScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.RoomsScore) + dto.RoomsScore;
            hotel.RoomsScore = totalScore / count;
        }
        private void UpdateReview_score(Hotel hotel)
        {
            var totalScore = hotel.LocationScore + hotel.ValueScore + hotel.ServiceScore + hotel.CleanlinessScore + hotel.RoomsScore;
            hotel.ReviewScore = totalScore / 5;
        }
    }
}
