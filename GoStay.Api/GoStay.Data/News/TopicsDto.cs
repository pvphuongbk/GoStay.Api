using GoStay.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.News
{
    public partial class TopicNewsDto
    {
        public int Id { get; set; }
        public string? Topic { get; set; }
        public int? Iddomain { get; set; }
        public string Tag { get; set; }=string.Empty;
    
    }
    public partial class NewsCategoryDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string? Slug { get; set; }
        public int Total { get; set; }
    }
}
