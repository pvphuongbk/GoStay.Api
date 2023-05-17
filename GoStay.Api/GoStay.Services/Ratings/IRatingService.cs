using GoStay.Data.Base;
using GoStay.DataDto.RatingDto;

namespace GoStay.Services.Ratings
{
    public interface IRatingService
    {
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto);
        public ResponseBase GetRatingByUser(int hotelId, int userId);
        public ResponseBase GetRatingByHotel(int hotelId);
        public ResponseBase GetUserBoxReview(int inHotel);
        public ResponseBase CheckOrdered(int hotelId, int userId);
        public ResponseBase UpdateStatusRating(int Id, byte status);
        public ResponseBase GetListRating(int? HotelId, byte? Status,string? NameSearch, int PageIndex, int PageSize);

    }
}
