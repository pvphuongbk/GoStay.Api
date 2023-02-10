using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.RatingDto;
using GoStay.Repository.Repositories;

namespace GoStay.Services.Reviews
{
    public class RatingService
    {
        private readonly ICommonRepository<HotelRating> _hotelRatingRepository;
        private readonly ICommonUoW _icommonUoWRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<HotelCriterion> _hotelCriterionRepository;

        public RatingService(ICommonRepository<HotelRating> hotelRatingRepository, ICommonUoW icommonUoWRepository, ICommonRepository<Hotel> hotelRepository, ICommonRepository<HotelCriterion> hotelCriterionRepository)
        {
            _hotelRatingRepository = hotelRatingRepository;
            _icommonUoWRepository = icommonUoWRepository;
            _hotelRepository = hotelRepository;
            _hotelCriterionRepository = hotelCriterionRepository;
        }

        public ResponseBase ReviewOrUpdateScore(int userId, int hotelId, List<RatingOrUpdateDto> ratingOrUpdateDtos)
        {
            ResponseBase response = new ResponseBase();
            var cids = ratingOrUpdateDtos.Select(x => x.IdCriteria).ToList();
            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == userId && x.IdHotel == hotelId && cids.Contains((int)x.IdCriteria));
            var criterias = _hotelCriterionRepository.FindAll().ToList();
            _icommonUoWRepository.BeginTransaction();
            List<HotelRating> items = new List<HotelRating>();
            foreach (var rating in ratingOrUpdateDtos)
            {
                var update = exitsRating.FirstOrDefault(x => x.IdCriteria == rating.IdCriteria);
                if(update != null)
                {
                    update.Point = (decimal)rating.Point;
                }
                else
                {
                    var item = new HotelRating
                    {
                        IdCriteria = rating.IdCriteria,
                        IdUser = userId,
                        IdHotel = hotelId,
                        Description = rating.Description,
                        DateReviews = DateTime.Now
                    };
                }
            }
            _hotelRatingRepository.UpdateMultiple(exitsRating);
            _hotelRatingRepository.InsertMultiple(items);
            _icommonUoWRepository.Commit();
            return response;
        }

        public bool UpdateScoreForHotel(int userId, int hotelId, List<RatingOrUpdateDto> ratingOrUpdateDtos, List<HotelCriterion> hotelCriterias)
        {
            var hotel = _hotelRepository.GetById(hotelId);
            if (hotel == null)
                return false;
            var otherRatings = _hotelRatingRepository.FindAll(x => x.IdUser != userId && x.IdHotel == hotelId).ToList();
            foreach(var criteria in hotelCriterias)
            {
                var adding = ratingOrUpdateDtos.FirstOrDefault(x => x.IdCriteria == criteria.Id);
                if(adding == null)
                    continue;
                var totalRatings = otherRatings.Count() + 1;
                var totalScore = otherRatings.Where(x => x.IdCriteria == criteria.Id).Sum(x => x.Point) + (decimal)adding.Point;



            }
            return true;
        }
    }
}
