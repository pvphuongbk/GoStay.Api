namespace GoStay.Data.TourDto
{
    public class SearchTourRequest
    {
        public int[]? IdTourTopic { get; set; }
        public int[]? IdTourStyle { get; set; }
        public int[]? IdProvinceFrom { get; set; }
        public int[]? IdProvinceTo { get; set; }
        public double? ActualPrice { get; set; }
        public int[]? Rating { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
