using GoStay.Data.TourDto;
using GoStay.DataDto.RatingDto;
using GoStay.Services.Ratings;
using GoStay.Services.Tours;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost("Rating")]
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto)
        {
            var items = _ratingService.ReviewOrUpdateScore(dto);
            return items;
        }

        [HttpGet("rating-by-user")]
        public ResponseBase GetRatingByUser(int hotelId, int userId)
        {
            var items = _ratingService.GetRatingByUser(hotelId, userId);
            return items;
        }

        [HttpGet("ordered")]
        public ResponseBase CheckOrdered(int hotelId, int userId)
        {
            var items = _ratingService.CheckOrdered(hotelId, userId);
            return items;
        }

        [HttpGet("rating-by-hotel")]
        public ResponseBase GetRatingByHotel(int hotelId)
        {
            var items = _ratingService.GetRatingByHotel(hotelId);
            return items;
        }
        [HttpGet("user-box-review")]
        public ResponseBase GetUserBoxReview(int idHotel)
        {
            var items = _ratingService.GetUserBoxReview(idHotel);
            return items;
        }
    }
}
