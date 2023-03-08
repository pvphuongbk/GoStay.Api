namespace GoStay.DataDto.Statistical
{
    public class PriceDetailByUserDto
    {
        public decimal TotalRoom { get; set; }
        public decimal TotalPriceRoom { get; set; }
        public decimal TotalPriceOrderSuccess { get; set; }
        public decimal TotalPriceOrder { get; set; }
    }
    public class PriceDetailByUserRequest
    {
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
