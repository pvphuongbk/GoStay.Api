using GoStay.Data.Enums;

namespace GoStay.Data.TourDto
{
    public class SearchTourDto
    {
        public int Id { get; set; }
        public string TourName { get; set; }
        public string TourStyle { get; set; }
        public string TourTopic { get; set; }
        public int Rating { get; set; }
        public int TourSize { get; set; }

        public string ProvinceFrom { get; set; }
        public List<string>? ProvinceTo { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public int Total { get; set; }
        public List<string>? Pictures { get; set; }
    }
}
