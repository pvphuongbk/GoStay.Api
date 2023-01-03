
using AutoMapper.Configuration.Conventions;
using GoStay.Common;
using GoStay.Data.OrderDto;
using GoStay.DataAccess.Entities;
using ResponseBase = GoStay.Data.Base.ResponseBase;
namespace GoStay.Services.Orders
{
    public interface IOrderService
    {
        public ResponseBase CreateOrder(OrderDto dataOrder, OrderDetailDto dataOrderRoom);
        public ResponseBase UpdateMoreInfoOrder(string moreinfo, int IdOrder);
        public ResponseBase AddOrderDetail(int IdOrder, OrderDetailDto orderDetail);
        public ResponseBase UpdateOrderDetail(InsertOrderDetailDto orderDetail);
        public ResponseBase RemoveOrderDetail(int IdOrderDetail);
        public ResponseBase UpdateStatusOrder(byte Status, int IdOder);
        public ResponseBase UpdatePTTTOrder(byte IdPTThanhtoan, int IdOder);
        public ResponseBase UpdateUserIDbySession(int IdUser, string Session);
        public ResponseBase GetOrderDetailbyOrder(int oder);
        public ResponseBase GetOrderbyUserID(int IDUser);
        public ResponseBase GetOrderbySession(string session);
        public ResponseBase CheckOrder(OrderDto order, OrderDetailDto orderDetai);
        public ResponseBase GetOrderbyId(int Id);

    }
}
