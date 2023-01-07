using GoStay.Data.TourDto;
using GoStay.Services.Tours;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;
        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpGet("suggest")]
        public ResponseBase SuggestTour(string searchText)
        {
            var items = _tourService.SuggestTour(searchText);
            return items;
        }

        [HttpPost("search")]
        public ResponseBase SearchTour(SearchTourRequest request)
        {
            var items = _tourService.SearchTour(request);
            return items;
        }
        [HttpGet("tourcontent")]
        public ResponseBase GetTourContent(int Id)
        {
            var items = _tourService.GetTourContent(Id);
            return items;
        }
    }
}
