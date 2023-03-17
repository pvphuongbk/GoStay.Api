namespace GoStay.Data.Ticket
{
    public class CreateOrderTicketParam
    {
        public OrderTicketDto order { get; set; }
        public OrderTicketDetailDto orderDetail { get; set; }
    }
    public class OrderTicketDto
    {
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public byte Status { get; set; }
        public byte IdPtthanhToan { get; set; }
        public string? Ordercode { get; set; }
        public string? Session { get; set; }
        public string DataFlightSession { get; set; } = null!;
        public string FlightSession { get; set; } = null!;
    }
    public class OrderTicketDetailDto
    {
        public int IdOrder { get; set; }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public string DepartureDateText { get; set; }
        public string StartDateText { get; set; }
        public string EndDateText { get; set; }
        public string AirlineCode { get; set; } = null!;
        public string AirlineName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public int Duration { get; set; }
        public string? Barrage { get; set; }
        public string? Class { get; set; }
        public double ServiceFee { get; set; }
        public double IssueFee { get; set; }
        public List<TicketPassengerDto> Passengers { get; set; }
    }
    public partial class TicketPassengerDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public bool Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Birthday { get; set; } = null!;
        public string? PassportExpiryDate { get; set; }
        public string? PassportIssueCountry { get; set; }
        public string? PassportNumber { get; set; }
        public int IdTicket { get; set; }
        public decimal Price { get; set; }
        public bool IsContact { get; set; }


    }

    public class OrderTicketShowDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public string DateCreateText { get; set; }
        public byte Status { get; set; }
        public string StatusText { get; set; }
        public byte IdPtthanhToan { get; set; }
        public string Paymentmethod { get; set; }
        public string? Ordercode { get; set; }
        public string? Session { get; set; }
        public string DataFlightSession { get; set; } = null!;
        public string FlightSession { get; set; } = null!;
        public OrderTicketDetailShowDto TicketDetail { get; set; }
    }
    public class OrderTicketDetailShowDto
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public string DateCreateText { get; set; }
        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public string DepartureDateText { get; set; }
        public string StartDateText { get; set; }
        public string EndDateText { get; set; }
        public string AirlineCode { get; set; } = null!;
        public string AirlineName { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public int Duration { get; set; }
        public string? Barrage { get; set; }
        public string? Class { get; set; }
        public double ServiceFee { get; set; }
        public double IssueFee { get; set; }
        public List<TicketPassengerShowDto> Passengers { get; set; }
    }
    public partial class TicketPassengerShowDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Type { get; set; }
        public bool Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Birthday { get; set; } = null!;
        public string? PassportExpiryDate { get; set; }
        public string? PassportIssueCountry { get; set; }
        public string? PassportNumber { get; set; }
        public int IdTicket { get; set; }
        public decimal Price { get; set; }
        public bool IsContact { get; set; }

    }
    public class OrderTicketAdminDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int IdUser { get; set; }
        public byte Status { get; set; }
        public string? Ordercode { get; set; }

        public int IdTicket { get; set; }
        public decimal Price { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public string AirlineName { get; set; } = null!;
        public DateTime DateCreate { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AirlineCode { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string? Barrage { get; set; }
        public string? Class { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }

    }
}
