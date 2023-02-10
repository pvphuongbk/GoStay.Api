using GoStay.Data.Base;
using GoStay.DataDto.RatingDto;

namespace GoStay.Services.Ratings
{
    public interface IRatingService
    {
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto);
        public ResponseBase GetRatingByUser(int hotelId, int userId);
    }
}
