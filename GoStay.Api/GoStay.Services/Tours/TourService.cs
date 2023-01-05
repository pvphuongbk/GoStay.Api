using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoStay.Services.Tours
{
    public class TourService : ITourService
    {
        private readonly ICommonRepository<Tour> _tourRepository;
        public TourService(ICommonRepository<Tour> tourRepository)
        {
            _tourRepository = tourRepository;
        }

        public ResponseBase SuggestTour(string searchText)
        {
            ResponseBase response = new ResponseBase();
            // Remove unicode
            searchText = searchText.RemoveUnicode();
            searchText = searchText.Replace(" ", string.Empty).ToLower();
            response.Data = TourRepository.SuggestTour(searchText);
            return response;
        }

        public ResponseBase SearchTour(SearchTourRequest request)
        {
            ResponseBase response = new ResponseBase();
            response.Data = TourRepository.GetPagingListTours(request);
            return response;
        }

        public ResponseBase GetTourContent(int Id)
        {
            
            ResponseBase response = new ResponseBase();
            response.Data = TourRepository.GetPagingListTours(request);
            return response;
        }
    }
}
