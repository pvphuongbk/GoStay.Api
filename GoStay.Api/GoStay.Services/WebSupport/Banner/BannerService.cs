using AutoMapper;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.Banner;
using GoStay.Repository.Repositories;

namespace GoStay.Services.WebSupport
{
    public class BannerService : IBannerService
    {
        private readonly ICommonRepository<Banner> _BannerRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;
        public BannerService(ICommonRepository<Banner> BannerRepository, ICommonUoW commonUoW, IMapper mapper)
        {
            _BannerRepository = BannerRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }
        public ResponseBase GetBannerDetail()
        {
            List<BannerDetailDto> listBanner = _mapper.Map<List<Banner>, List<BannerDetailDto>>(_BannerRepository.FindAll(x => x.ParentId != 0).ToList());
            ResponseBase response = new ResponseBase();
            response.Data = listBanner;
            return response;
        }
    }
}
