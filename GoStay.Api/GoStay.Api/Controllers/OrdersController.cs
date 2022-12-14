using GoStay.Common;
using GoStay.Data.OrderDto;
using GoStay.DataAccess.Entities;
using GoStay.Services.Hotels;
using GoStay.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using ResponseBase = GoStay.Data.Base.ResponseBase;

namespace GoStay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("order")]
        public ResponseBase CreateOrder(CreateOrderParam order)
        {
            var items = _orderService.CreateOrder(order.order, order.orderDetail);
            return items;
        }

        [HttpPost("check-order")]
        public ResponseBase CheckOrder(CreateOrderParam order)
        {
            var items = _orderService.CheckOrder(order.order, order.orderDetail);
            return items;
        }

        [HttpPost("detail")]
        public ResponseBase AddOrderDetail(AddOrderDetailParam dto)
        {
            var items = _orderService.AddOrderDetail(dto.IdOrder, dto.orderDetail);
            return items;
        }
        [HttpPut("detail")]
        public ResponseBase UpdateOrderDetail(InsertOrderDetailDto orderDetail)
        {
            var items = _orderService.UpdateOrderDetail(orderDetail);
            return items;
        }
        [HttpDelete("detail")]
        public ResponseBase RemoveOrderDetail(int IdOrderDetail)
        {
            var items = _orderService.RemoveOrderDetail(IdOrderDetail);
            return items;
        }
        [HttpPut("status")]
        public ResponseBase UpdateStatusOrder(UpdateStatusOrderParam param)
        {
            var items = _orderService.UpdateStatusOrder(param.Status, param.IdOder);
            return items;
        }
        [HttpPut("pttt")]
        public ResponseBase UpdatePTTTOrder(UpdatePTTTOrderParam param)
        {
            var items = _orderService.UpdatePTTTOrder(param.IdPTThanhtoan, param.IdOder);
            return items;
        }
        [HttpPut("moreinfo")]
        public ResponseBase UpdateMoreInfoOrder(UpdateMoreInfoOrderParam info)
        {
            var items = _orderService.UpdateMoreInfoOrder(info.moreinfo, info.IdOrder);
            return items;
        }

        [HttpPut("userid-by-session")]
        public ResponseBase UpdateUserIDbySession(UpdateUserIdBySessionParam param )
        {
            var items = _orderService.UpdateUserIDbySession(param.IdUser, param.Session);
            return items;
        }

        [HttpGet("detail-by-order")]
        public ResponseBase GetOrderDetailbyOrder(int order)
        {
            var items = _orderService.GetOrderDetailbyOrder(order);
            return items;
        }
        [HttpGet("order-by-userid")]
        public ResponseBase GetOrderbyUserID(int IDUser)
        {
            var items = _orderService.GetOrderbyUserID(IDUser);
            return items;
        }
        [HttpGet("order-by-session")]
        public ResponseBase GetOrderbySession(string session)
        {
            //var items = _orderService.GetOrderbySession(session);
            var items2 = _orderService.GetOrderbySession2(session);
            return items2;
        }
        [HttpGet("order-by-id")]
        public ResponseBase GetOrderbyId(int Id)
        {
            var items = _orderService.GetOrderbyId(Id);
            return items;
        }

        [HttpGet("order-in-month")]
        public ResponseBase GetOrderByMonth(int month, int year, int status)
        {
            var items = _orderService.GetOrderByMonth(month, year, status);
            return items;
        }

        [HttpGet("money-in-month")]
        public ResponseBase GetOrderTotalMoneyByMonth(int month, int year, int status)
        {
            var items = _orderService.GetOrderTotalMoneyByMonth(month, year, status);
            return items;
        }

        [HttpGet("room-in-month")]
        public ResponseBase GetOrderRoomByMonth(int month, int year, int status)
        {
            var items = _orderService.GetOrderRoomByMonth(month, year, status);
            return items;
        }

    }
}
