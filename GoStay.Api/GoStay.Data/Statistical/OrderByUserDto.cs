namespace GoStay.DataDto.Statistical
{
    public class OrderByUserDto
    {
        public int Id { get; set; }
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
        public int TotalPage{ get; set; }

        public string UserName { get; set; }
        public string Ordercode { get; set; }

        public string? RoomNames
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    ListRoomNames = new List<string>();
                else
                {
                    ListRoomNames = value.Split(';').ToList();

                }
            }
        }
        public List<string> ListRoomNames { get; set; } = new List<string>();
        public string? PaymentMethod { get; set; }
        public int? IdHotel { get; set; }
        public string? SlugHotel { get; set; }
    }
}