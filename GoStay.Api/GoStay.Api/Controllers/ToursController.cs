using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
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

        [HttpGet("tour-homepage")]
        public ResponseBase GetTourHomePage()
        {
            var items = _tourService.GetTourHomePage();
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
        [HttpGet("tour-location")]
        public ResponseBase GetTourLocationTotal(int IdProvince)
        {
            var items = _tourService.GetTourLocationTotal(IdProvince);
            return items;
        }
        [HttpGet("data-support")]
        public ResponseBase GetDataSupportTour()
        {
            var items = _tourService.GetDataSupportTour();
            return items;
        }
        [HttpGet("list-tour-by-userid")]
        public ResponseBase GetAllTourByUserId(int UserId, int PageIndex, int PageSize)
        {
            var items = _tourService.GetAllTourByUserId( UserId,  PageIndex,  PageSize);
            return items;
        }

        [HttpPost("add-tour")]
        public ResponseBase AddTour(TourAddParam param)
        {
            var items = _tourService.AddTour(param.tourAddDto, param.IdDistrictTo, param.Vehicles);
            return items;
        }
        [HttpPost("add-tour-detail")]
        public ResponseBase AddTourDetail(TourDetail data)
        {
            var items = _tourService.AddTourDetail(data);
            return items;
        }
        [HttpPut("edit-tour")]
        public ResponseBase EditTour(TourAddDto data)
        {
            var items = _tourService.EditTour(data);
            return items;
        }
        [HttpPut("edit-tour-detail")]
        public ResponseBase EditTourDetail(TourDetail data)
        {
            var items = _tourService.EditTourDetail(data);
            return items;
        }
        [HttpDelete("delete-tour")]
        public ResponseBase DeleteTour(int Id)
        {
            var items = _tourService.DeleteTour(Id);
            return items;
        }
        [HttpDelete("remove-tour-detail")]
        public ResponseBase RemoveTourDetail(int IdDetail)
        {
            var items = _tourService.RemoveTourDetail(IdDetail);
            return items;
        }
        [HttpPost("update-tour-to-compare")]
        public ResponseBase UpdateTourToCompare(CompareTourParam param)
        {
            var items = _tourService.UpdateTourToCompare(param);
            return items;
        }
        [HttpGet("list-tour-compare")]
        public ResponseBase GetListToursCompare(string listId)
        {
            var items = _tourService.GetListToursCompare(listId);
            return items;
        }
    }
}
