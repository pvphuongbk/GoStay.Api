﻿using GoStay.Data.Base;
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
        ResponseBase EditTourDetail(TourDetailDto data);
        ResponseBase DeleteTour(int id);
        ResponseBase RemoveTourDetail(int IdDetail);
        ResponseBase UpdateTourToCompare(CompareTourParam param);
        ResponseBase GetListToursCompare(string ListId);
        public ResponseBase GetTourByUserIdAndId(int UserId, int Id);
        public int AddTourStartTime(string time);
        public ResponseBase SavePicture(string url, int idTour, int size);
        public ResponseBase SaveListPicture(List<string> datas);
        public ResponseBase GetListTourDetail(int IdTour);
        public ResponseBase GetListPictureTour(int IdTour);
        public ResponseBase DeletePictureTour(int IdPicture);
    }
}
