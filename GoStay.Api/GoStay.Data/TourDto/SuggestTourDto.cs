using GoStay.Data.Enums;

namespace GoStay.Data.TourDto
{
    public class SuggestTourDto
    {
        public int Count { get; set; }
        public string TenTT { get; set; }
        public int Id { get; set; }
        public SuggestTourType Type { get; set; }
    }
}
