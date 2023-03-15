using GoStay.Data.OrderDto;
using GoStay.Data.Ticket;
using GoStay.DataDto.OrderDto;
using GoStay.Services.Orders;
using GoStay.Services.OrderTickets;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersTicketController : ControllerBase
    {
        private readonly IOrderTicketService _orderService;
        public OrdersTicketController(IOrderTicketService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("order-ticket")]
        public ResponseBase CreateOrderTicket(CreateOrderTicketParam param)
        {
            var items = _orderService.CreateOrderTicket(param.order, param.orderDetail);
            return items;
        }

        [HttpPost("check-order-ticket")]
        public ResponseBase CheckOrderTicket(CreateOrderTicketParam order)
        {
            var items = _orderService.CheckOrderTicket(order.order, order.orderDetail);
            return items;
        }

        [HttpGet("order-ticket-by-id")]
        public ResponseBase GetOrderTicketbyId(int Id)
        {
            var items = _orderService.GetOrderTicketbyId(Id);
            return items;
        }

    }
}
