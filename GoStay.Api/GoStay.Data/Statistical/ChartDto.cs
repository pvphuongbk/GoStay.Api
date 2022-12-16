namespace GoStay.Data.Statistical
{
    public class ChartDto
    {
        public ChartDto()
        {
            HotelRating = new Dictionary<string, ChartValue>();
            PriceRange = new Dictionary<string, ChartValue>();
            TypeHotel = new Dictionary<string, ChartValue>();
            RoomByMonth = new Dictionary<string, ChartValue>();
        }

        public int TotalHotel { get; set; }
        public int TotalRoom { get; set; }
        public int TotalImg { get; set; }
        public long TotalSizeImg { get; set; }
        public Dictionary<string, ChartValue> HotelRating { get; set; }
        public Dictionary<string, ChartValue> PriceRange { get; set; }
        public Dictionary<string, ChartValue> TypeHotel { get; set; }
        public Dictionary<string, ChartValue> RoomByMonth { get; set; }
    }
    public class HotelTypeForChartDto
    {
        public int Count { get; set; }
        public string? Type { get; set; }
    }
    public class PriceRangeForChartDto
    {
        public int Count { get; set; }
        public string? Title { get; set; }
    }

    public class HotelRatingForChartDto
    {
        public int? Rating { get; set; }
    }

    public class CreateRoomForChartDto
    {
        public  DateTime CreatedDate { get; set; }
    }

    public class ChartValue
    { 
        public double Percent { get; set; }
        public int Count { get; set; }
    }
}
