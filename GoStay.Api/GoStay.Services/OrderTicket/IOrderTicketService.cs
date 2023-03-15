using GoStay.Data.Base;
using GoStay.Data.Ticket;

namespace GoStay.Services.OrderTickets
{
    public interface IOrderTicketService
    {
        public ResponseBase CreateOrderTicket(OrderTicketDto order, OrderTicketDetailDto orderDetail);
        public ResponseBase CheckOrderTicket(OrderTicketDto order, OrderTicketDetailDto orderDetail);
        public ResponseBase GetOrderTicketbyId(int Id);
    }
}
