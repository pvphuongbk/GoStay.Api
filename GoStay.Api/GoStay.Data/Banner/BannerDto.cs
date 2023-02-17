using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Banner
{
    public class BannerPageDto
    {
        public List<BannerDetailDto> BannerDetails { get; set; }
    }
    public class BannerDetailDto
    {
        public int ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
    }
}
