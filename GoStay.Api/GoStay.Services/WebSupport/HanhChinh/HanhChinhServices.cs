
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using Microsoft.EntityFrameworkCore;
using GoStay.Common.Configuration;
using GoStay.DataDto.Hành_Chính;
using AutoMapper;
using System.Collections.Generic;
using GoStay.Data.Base;
using GoStay.DataDto;

namespace GoStay.Services.WebSupport
{

    public class ProvinceService : IProvinceService
    {
        private readonly ICommonRepository<TinhThanh> _TinhRepository;
        private IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public ProvinceService(ICommonRepository<TinhThanh> tinhRepository, ICommonUoW commonUoW, IMapper mapper)
        {
            _TinhRepository = tinhRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }

        public ResponseBase GetAllProvince(int? IdCountry=1)
        {
            ResponseBase response = new ResponseBase();
            var result  = _TinhRepository.FindAll(x=>x.IdCountry== IdCountry)
                .OrderBy(x => x.TenTt).OrderBy(x => x.Stt).ToList();
            response.Data = result;
            return response;
        }


        public ResponseBase GetProvinceNameById(int Id)
        {
            ResponseBase response = new ResponseBase();

            var provinceName = _TinhRepository.FindAll(x => x.Id == Id).Select(x => x.TenTt).FirstOrDefault();
            if(provinceName is null)
            {
                provinceName = "Việt Nam";
            }
            response.Data = provinceName;
            return response;
            ;
        }
        public ResponseBase GetTopProvince()
        {
            ResponseBase response = new ResponseBase();
            response.Data = _mapper.Map<List<TinhThanh>, List<TinhThanhBannerDto>>( _TinhRepository.FindAll().OrderBy(x=>x.Stt).Take(16).ToList());
            return response;
        }
    }

    public class DistrictService : IDistrictService
    {
        private readonly ICommonRepository<Quan> _QuanRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourDistrictRepository;
        private IMapper _mapper;

        private readonly ICommonUoW _commonUoW;
        public DistrictService(ICommonRepository<Quan> QuanRepository, ICommonUoW commonUoW, ICommonRepository<TourDistrictTo> tourDistrictRepository
                                , IMapper mapper)
        {
            _QuanRepository = QuanRepository;
            _commonUoW = commonUoW;
            _tourDistrictRepository = tourDistrictRepository;
            _mapper = mapper;
        }
        public ResponseBase GetDistrictById(int Id)
        {
            ResponseBase response = new ResponseBase();
            response.Data= _QuanRepository.GetById(Id);
            return response;
        }

        public ResponseBase GetAllDistrict()
        {
            ResponseBase response = new ResponseBase();

            var quans= _QuanRepository.FindAll().Include(x=>x.IdTinhThanhNavigation).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).ToList();
            var quansdto = _mapper.Map<List<Quan>, List<QuanDto>>(quans);
            quansdto.ForEach(x => x.ProvinceName = quans.Where(y => y.Id == x.Id).Single().IdTinhThanhNavigation.TenTt);
            response.Data= quansdto;
            return response;
        }

        public ResponseBase ProvinceFromForTour()
        {
            ResponseBase response = new ResponseBase();
            var result  = _QuanRepository.FindAll(x => x.Tours.Any()).Include(x => x.IdTinhThanhNavigation)
                .Select(x=>x.IdTinhThanhNavigation).OrderBy(x=>x.Stt).ToList();
            response.Data= result;
            return response;
        }

        public ResponseBase ProvinceToForTour()
        {
            ResponseBase response = new ResponseBase();
            var result = _tourDistrictRepository.FindAll().Include(x=>x.IdDistrictToNavigation)
                .ThenInclude(x=>x.IdTinhThanhNavigation).Select(x=>x.IdDistrictToNavigation.IdTinhThanhNavigation)
                .OrderBy(x => x.Stt).Distinct().ToList();
            response.Data = result;
            return response;
        }

        public ResponseBase GetProvinceNameByDistrictId(int districtId)
        {
            ResponseBase response = new ResponseBase();
            response.Data = _QuanRepository.FindAll(x => x.Id == districtId).Include(x=>x.IdTinhThanhNavigation).SingleOrDefault().IdTinhThanhNavigation.TenTt;
            return response;
        }

        public ResponseBase GetDistrictByProvinceId(int provinceId)
        {
            ResponseBase response = new ResponseBase();
            var result = _QuanRepository.FindAll(x => x.IdTinhThanh == provinceId).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).ToList();
            response.Data= result;
            return response;
        }
        public ResponseBase GetDistrictIdsByProvinceIds(int[] provinceId)
        {
            ResponseBase response = new ResponseBase();
            List<int> district = new List<int>();
            foreach (var id in provinceId)
            {
                district.AddRange(_QuanRepository.FindAll(x => x.IdTinhThanh == id).OrderBy(x => x.Tenquan).OrderBy(x => x.Stt).Select(x => x.Id).ToList());
            }
            response.Data = district.ToArray();
            return response;
        }

    }

    public class WardService : IWardService
    {
        private readonly ICommonRepository<Phuong> _PhuongRepository;
        private readonly ICommonRepository<Quan> _QuanRepository;
        private readonly ICommonUoW _commonUoW;
        public WardService(ICommonRepository<Phuong> PhuongRepository, ICommonUoW commonUoW, ICommonRepository<Quan> quanRepository)
        {
            _PhuongRepository = PhuongRepository;
            _commonUoW = commonUoW;
            _QuanRepository = quanRepository;
        }

        public ResponseBase GetAllWards()
        {
            ResponseBase response = new ResponseBase();

            response.Data = _PhuongRepository.FindAll(x => x.Stt > 0)
                //.Include(x=>x.IdQuanNavigation)
                .OrderBy(x => x.Stt).OrderBy(x => x.Tenphuong).ToList();
            return response; 
        }
        public ResponseBase GetWardsByIdDistrict(int IdQuanHuyen)
        {
            ResponseBase response = new ResponseBase();

            var result  = _PhuongRepository.FindAll(x => x.Stt > 0 && x.IdQuan == IdQuanHuyen)
                .OrderBy(x => x.Stt).OrderBy(x => x.Tenphuong).ToList();
            response.Data=result;
            return response;
        }
        public ResponseBase GetIdDistrictByIdWard(int IdPhuong)
        {
            ResponseBase response = new ResponseBase();
            response.Data = _PhuongRepository.FindAll().FirstOrDefault(x => x.Id == IdPhuong).IdQuan;
            return response;
        }
    }
}
