using GoStay.Common;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Data.Base;
using ResponseBase = GoStay.Data.Base.ResponseBase;
using ErrorCodeMessage = GoStay.Data.Base.ErrorCodeMessage;
using GoStay.DataAccess.UnitOfWork;
using System.Collections.Generic;
using AutoMapper;
using GoStay.Data.OrderDto;

namespace GoStay.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICommonRepository<Order> _OrderRepository;
        private readonly ICommonRepository<OrderDetail> _OrderDetailRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;


        public OrderService(ICommonRepository<Order> OrderRepository,ICommonRepository<OrderDetail> OrderRoomRepository,ICommonUoW commonUoW,
            IMapper mapper)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderRepository = OrderRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
        }

        public ResponseBase CreateOrder(OrderDto dataOrder, OrderDetailDto dataOrderRoom)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var orderEntity = _mapper.Map<OrderDto, Order>(dataOrder);
                _OrderRepository.Insert(orderEntity);
                dataOrderRoom.IdOrder = orderEntity.Id;

                var orderDetailEntity = _mapper.Map<OrderDetailDto, OrderDetail>(dataOrderRoom);
                _OrderDetailRepository.Insert(orderDetailEntity);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpdateOrder(Order dataOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                dataOrder.DateUpdate = DateTime.Now;
                _OrderRepository.Update(dataOrder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase AddOrderDetail(int IdOrder, OrderDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                orderDetail.IdOrder = IdOrder;
                var orderDetailEntity = _mapper.Map<OrderDetailDto, OrderDetail>(orderDetail);
                _OrderDetailRepository.Insert(orderDetailEntity);
                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpdateOrderDetail(OrderDetail orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                _OrderDetailRepository.Update(orderDetail);
                var oder = _OrderRepository.GetById(orderDetail.IdOrder);
                oder.DateUpdate = DateTime.Now;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        
        public ResponseBase RemoveOrderDetail(int IdOrderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var orderDetail = _OrderDetailRepository.GetById(IdOrderDetail);
                var oder = _OrderRepository.GetById(orderDetail.IdOrder);
                oder.DateUpdate = DateTime.Now;
                oder.OrderDetails.Remove(orderDetail);
                _OrderRepository.Update(oder);
                orderDetail.IsDeleted = true;
                _OrderDetailRepository.Update(orderDetail);

                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpdateStatusOrder(byte Status, int IdOder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOder);
                oder.DateUpdate = DateTime.Now;
                oder.Status = Status;
                _OrderRepository.Update(oder);

                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpdatePTTTOrder(byte IdPTThanhtoan, int IdOder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOder);
                oder.DateUpdate = DateTime.Now;
                oder.IdPtthanhToan = IdPTThanhtoan;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch (Exception e)
            {
                _commonUoW.RollBack();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase GetOrderDetailbyOrder(int oder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var list = _OrderRepository.GetById(oder).OrderDetails.ToList();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = list;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetOrderbyUserID(int IDUser)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var list = _OrderRepository.FindAll(x=>x.IdUser==IDUser).ToList();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data=list;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetOrderbySession(string session)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var order = _OrderRepository.FindAll(x => x.Session == session).FirstOrDefault();
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = order;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
    }
}
