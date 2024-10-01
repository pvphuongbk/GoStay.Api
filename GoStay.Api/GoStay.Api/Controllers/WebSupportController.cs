
using GoStay.Api.Attributes;
using GoStay.Data.Base;
using GoStay.Services.WebSupport;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;
namespace GoStay.Api.Controllers
{
	[ApiController]
    [Authorize]
    [Route("[controller]")]
	public class WebSupportController : ControllerBase
	{

        private readonly IBannerService _bannerService;
        private readonly IProvinceService _provinceService;
        private readonly IDistrictService _districtService;
        private readonly IWardService _wardService;


        public WebSupportController(IBannerService bannerService, IProvinceService provinceService, 
            IDistrictService districtService, IWardService wardService)
		{
			_bannerService = bannerService;
            _provinceService = provinceService;
            _districtService = districtService;
            _wardService = wardService;

        }
        //Banner
        [HttpGet("banner-detail")]
        public ResponseBase GetBannerDetail()
        {
            var items = _bannerService.GetBannerDetail();
            return items;
        }

        //Province
        [HttpGet("provinces")]
        public ResponseBase GetAllProvince(int? IdCountry=1)
        {
            var items = _provinceService.GetAllProvince(IdCountry);
            return items;
        }
        [HttpGet("province-name-by-id")]
        public ResponseBase GetProvinceNameById(int Id)
        {
            var items = _provinceService.GetProvinceNameById(Id);
            return items;
        }
        [HttpGet("top-provinces")]
        public ResponseBase GetTopProvince()
        {
            var items = _provinceService.GetTopProvince();
            return items;
        }

        //District
        [HttpGet("district-by-id")]
        public ResponseBase GetDistrictById(int Id)
        {
            var items = _districtService.GetDistrictById(Id);
            return items;
        }
        [HttpGet("districts")]
        public ResponseBase GetAllDistrict()
        {
            var items = _districtService.GetAllDistrict();
            return items;
        }
        [HttpGet("provincefrom-for-tour")]
        public ResponseBase ProvinceFromForTour()
        {
            var items = _districtService.ProvinceFromForTour();
            return items;
        }
        [HttpGet("provinceto-for-tour")]
        public ResponseBase ProvinceToForTour()
        {
            var items = _districtService.ProvinceToForTour();
            return items;
        }
        [HttpGet("province-name-by-district-id")]
        public ResponseBase GetProvinceNameByDistrictId(int Id)
        {
            var items = _districtService.GetProvinceNameByDistrictId(Id);
            return items;
        }
        [HttpGet("districts-by-province-id")]
        public ResponseBase GetDistrictByProvinceId(int Id)
        {
            var items = _districtService.GetDistrictByProvinceId(Id);
            return items;
        }
        [HttpGet("districtids-by-provinceids")]
        public ResponseBase GetDistrictIdsByProvinceIds(int[] Id)
        {
            var items = _districtService.GetDistrictIdsByProvinceIds(Id);
            return items;
        }

        //Wards
        [HttpGet("wards")]
        public ResponseBase GetAllWards()
        {
            var items = _wardService.GetAllWards();
            return items;
        }
        [HttpGet("wards-by-district-id")]
        public ResponseBase GetWardsByIdDistrict(int Id)
        {
            var items = _wardService.GetWardsByIdDistrict(Id);
            return items;
        }
        [HttpGet("district-id-by-ward-id")]
        public ResponseBase GetIdDistrictByIdWard(int Id)
        {
            var items = _wardService.GetIdDistrictByIdWard(Id);
            return items;
        }

    }
}