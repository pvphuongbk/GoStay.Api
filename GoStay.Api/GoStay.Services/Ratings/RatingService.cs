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

        public RatingService(ICommonRepository<HotelRating> hotelRatingRepository)
        {
            _hotelRatingRepository = hotelRatingRepository;
        }

        public ResponseBase ReviewOrUpdateScore(int userId, int hotelId, List<RatingOrUpdateDto> ratingOrUpdateDtos)
        {
            ResponseBase response = new ResponseBase();
            var cids = ratingOrUpdateDtos.Select(x => x.IdCriteria).ToList();
            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == userId && x.IdHotel == hotelId && cids.Contains((int)x.IdCriteria));
            //foreach (var rating in ratingOrUpdateDtos)
            //{

            //}
            return response;
        }
    }
}
