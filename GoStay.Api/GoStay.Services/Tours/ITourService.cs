using GoStay.Data.Base;
using GoStay.Data.TourDto;

namespace GoStay.Services.Tours
{
    public interface ITourService
    {
        ResponseBase SuggestTour(string searchText);
        ResponseBase SearchTour(SearchTourRequest request);
        public ResponseBase GetTourContent(int Id);
        public ResponseBase GetTourHomePage();
        public ResponseBase GetTourLocationTotal(int IdProvince);

    }
}
