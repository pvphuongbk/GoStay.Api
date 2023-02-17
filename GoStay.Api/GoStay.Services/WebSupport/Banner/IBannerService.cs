using GoStay.Data.Base;
using GoStay.DataDto.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.Services.WebSupport
{
    public interface IBannerService
    {
        public ResponseBase GetBannerDetail();

    }
}
