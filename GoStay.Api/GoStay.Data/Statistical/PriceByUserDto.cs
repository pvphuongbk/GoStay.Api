namespace GoStay.DataDto.Statistical
{
    public class PriceByUserDto
    {
        public decimal Value { get; set; }
        public int Status { get; set; }
    }

    public class PriceDetailByUserDto
    {
        public decimal HandlingValue { get; set; }
        public decimal TotalValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal PendingValue { get; set; }
    }
}
