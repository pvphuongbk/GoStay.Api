using AutoMapper;
using GoStay.Common.Extention;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.Services.Tours;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly IMapper _mapper;
        public ToursController(ITourService tourService, IMapper mapper)
        {
            _tourService = tourService;
            _mapper = mapper;
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
        [HttpGet("tour-by-id")]
        public ResponseBase GetTourByUserIdAndId(int UserId, int Id)
        {
            var items = _tourService.GetTourByUserIdAndId(UserId, Id);
            return items;
        }
        [HttpPost("add-tour")]
        public ResponseBase AddTour(TourAddDto tourAdd)
        {
            var tour = new Tour();
            tourAdd.TourName = tourAdd.TourName.Trim();
            tour = _mapper.Map<TourAddDto, Tour>(tourAdd);
            tour.StartDate = new DateTime(2100, 1, 1);

            if (tourAdd.StartDateString != "" && tourAdd.IdStartTime == null)
            {
                tourAdd.StartDateString = tourAdd.StartDateString.Trim();
                try
                {
                    tour.StartDate = DateTime.ParseExact(tourAdd.StartDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    tour.IdStartTime = _tourService.AddTourStartTime(tourAdd.StartDateString);
                }
            }

            if (tour.Discount is null)
                tour.Discount = 0;
            tour.Status = 1;
            tour.ActualPrice = tour.Price * (100 - (double)tour.Discount) / 100;
            tour.SearchKey = tour.TourName.RemoveUnicode();
            tour.SearchKey = tour.SearchKey.Replace(" ", string.Empty).ToLower();
            var items = _tourService.AddTour(tour, tourAdd.IdDistrictTo, tourAdd.Vehicle);
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

        [HttpPost("save-picture-tour")]
        public ResponseBase SavePicture(string url, int IdTour, int size)
        {
            var items = _tourService.SavePicture(url, IdTour,size);
            return items;
        }
        [HttpPost("save-listpicture-tour")]
        public ResponseBase SaveListPicture(List<string> data)
        {
            var items = _tourService.SaveListPicture(data);
            return items;
        }
    }
}
