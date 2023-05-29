using GoStay.Data.Base;
using GoStay.Data.TourDto;
using GoStay.DataAccess.Entities;

namespace GoStay.Services.Tours
{
    public interface ITourService
    {
        ResponseBase SuggestTour(string searchText);
        ResponseBase SearchTour(SearchTourRequest request);
        ResponseBase GetTourContent(int Id);
        ResponseBase GetTourHomePage();
        ResponseBase GetTourLocationTotal(int IdProvince);
        ResponseBase GetDataSupportTour();
        ResponseBase GetAllTourByUserId(int UserId, int PageIndex, int PageSize);
        ResponseBase AddTour(Tour data, int[] IdDistrictTo, int[] Vehicles);
        ResponseBase AddTourDetail(TourDetail data);
        ResponseBase EditTour(TourAddDto data);
        ResponseBase EditTourDetail(TourDetail data);
        ResponseBase DeleteTour(int id);
        ResponseBase RemoveTourDetail(int IdDetail);
        ResponseBase UpdateTourToCompare(CompareTourParam param);
        ResponseBase GetListToursCompare(string ListId);

    }
}
