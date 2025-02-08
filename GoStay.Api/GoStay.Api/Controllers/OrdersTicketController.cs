using GoStay.Api.Attributes;
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
    [Authorize]
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
        [HttpGet("all-order-ticket")]
        public ResponseBase GetAllOrderTicket(int? UserId,int pageIndex, int pageSize)
        {
            var items = _orderService.GetAllOrderTicket(UserId,pageIndex, pageSize);
            return items;
        }
        [HttpPut("update-status")]
        public ResponseBase UpdateStatus(UpdateStatus param)
        {
            var items = _orderService.UpdateStatus(param.UserId, param.TicketId );
            return items;
        }
    }
}
