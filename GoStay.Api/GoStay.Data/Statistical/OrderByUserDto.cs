namespace GoStay.DataDto.Statistical
{
    public class OrderByUserDto
    {
        public int RowNum { get; set; }
        public decimal Total { get; set; }
        public int Type { get; set; }
        public string HotelName { get; set; }
        public int Status { get; set; }
        public DateTime DateCreate { get; set; }
    }
}