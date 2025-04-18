﻿
using AutoMapper.Configuration.Conventions;
using GoStay.Common;
using GoStay.Data.OrderDto;
using GoStay.DataAccess.Entities;
using GoStay.DataDto.OrderDto;
using ResponseBase = GoStay.Data.Base.ResponseBase;
namespace GoStay.Services.Orders
{
    public interface IOrderService
    {
        public ResponseBase CreateOrder(OrderDto dataOrder);
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
        ResponseBase GetOrderbySession2(string session);
        public ResponseBase CheckOrder(OrderDto order);
        public ResponseBase GetOrderbyId(int Id);
        ResponseBase GetOrderTotalMoneyByMonth(int month, int year, int status);
        ResponseBase GetOrderByMonth(int month, int year, int status);
        ResponseBase GetOrderRoomByMonth(int month, int year, int status);
        ResponseBase GetListOrderSearch(OrderSearchParam param);
        public ResponseBase DeleteRoomInOrder(int IdRoom, int IdOrder);
        public ResponseBase GetRoomInOrder(int Id);
        public ResponseBase UpdatePrePayment(decimal prepayment, int IdOrder);

        public ResponseBase UpdateTotalAmount(UpdateTotalAmountOrderParam param);
        public ResponseBase UpdateOrder(OrderDto order,int IdOrder);
        public ResponseBase GetBookedDateRoom(int IdRoom);
        public ResponseBase RejectOrder(int IdOrder, int IdUser);
        public ResponseBase UpdateBookedDateHotel(int idOrder);
    }
}
