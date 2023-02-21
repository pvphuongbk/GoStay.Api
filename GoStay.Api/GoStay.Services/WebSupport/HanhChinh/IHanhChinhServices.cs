
using GoStay.Common;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.Hành_Chính;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Services.WebSupport
{
    public interface IProvinceService
    {
        ResponseBase GetAllProvince(int? IdCountry=1);
        ResponseBase GetProvinceNameById(int IDTinhThanh);
        ResponseBase GetTopProvince();
    }
    public interface IDistrictService
    {
        ResponseBase GetDistrictById(int Id);
        ResponseBase GetAllDistrict();
        ResponseBase ProvinceFromForTour();
        ResponseBase ProvinceToForTour();
        ResponseBase GetProvinceNameByDistrictId(int districtId);
        ResponseBase GetDistrictByProvinceId(int provinceId);
        ResponseBase GetDistrictIdsByProvinceIds(int[] provinceId);
    }

    public interface IWardService
    {
        ResponseBase GetAllWards();
        ResponseBase GetWardsByIdDistrict(int IdQuanHuyen);
        ResponseBase GetIdDistrictByIdWard(int IdPhuong);
    }
}
