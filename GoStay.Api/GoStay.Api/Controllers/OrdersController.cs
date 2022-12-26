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

        [HttpGet("check-order")]
        public ResponseBase CheckOrder(int iduser, int idhotel, int IdRoom)
        {
            var items = _orderService.CheckOrder(iduser, idhotel,IdRoom);
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
            var items = _orderService.GetOrderbySession(session);
            return items;
        }

    }
}
