using GoStay.Data.HotelDto;
using GoStay.DataDto.RatingDto;

namespace GoStay.DataDto.HotelDto
{
    public class HotelDetailSummaryDto
    {
        public HotelDetailDto HotelDetailDto { get; set; }
        public List<UserBoxReview> UserBoxReviews { get; set; }
    }
}
