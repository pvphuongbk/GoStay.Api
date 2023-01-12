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

        public string? DistrictFrom { get; set; }
        public List<string>? DistrictTo { get; set; }

        public DateTime StartDate { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public double ActualPrice { get; set; }
        public int Total { get; set; }
        private string Urls
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Pictures = new List<string>();
                else
                {
                    Pictures = value.Split(';').ToList();

                }
            }
        }
        public List<string> Pictures { get; set; } = new List<string>();
    }
}
