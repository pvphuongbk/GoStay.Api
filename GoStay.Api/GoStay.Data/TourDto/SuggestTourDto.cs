using GoStay.Data.Enums;
using GoStay.DataDto.Helper;

namespace GoStay.Data.TourDto
{
    public class SuggestTourDto
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public SuggestTourType Type { get; set; }
        public string Description
        {
            get
            {
                return Type.GetEnumDescription();
            }
        }
        public string Slug { get; set; }
    }
}
