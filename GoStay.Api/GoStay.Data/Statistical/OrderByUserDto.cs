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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public int TotalCount { get; set; }

    }
}