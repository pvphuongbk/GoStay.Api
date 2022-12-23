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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using GoStay.Common.Helpers.Hotels;
using GoStay.Data.ServiceDto;
using GoStay.Repository.Repositories;

namespace GoStay.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICommonRepository<Order> _OrderRepository;
        private readonly ICommonRepository<OrderDetail> _OrderDetailRepository;
        private readonly ICommonRepository<Tour> _tourRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;


        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;


        public OrderService(ICommonRepository<Order> OrderRepository,ICommonRepository<OrderDetail> OrderRoomRepository,ICommonUoW commonUoW,
            IMapper mapper, ICommonRepository<Tour> tourRepository, ICommonRepository<HotelRoom> roomRepository)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderRepository = OrderRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _tourRepository = tourRepository;
            _roomRepository = roomRepository;
        }

        public ResponseBase CreateOrder(OrderDto order, OrderDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var orderEntity = _mapper.Map<OrderDto, Order>(order);
                _OrderRepository.Insert(orderEntity);
                _commonUoW.Commit();
                _commonUoW.BeginTransaction();

                orderDetail.IdOrder = orderEntity.Id;

                var orderDetailEntity = _mapper.Map<OrderDetailDto, OrderDetail>(orderDetail);
                if (orderDetail.DetailStyle == 1)
                {
                    orderDetailEntity.IdRoom = orderDetail.IdProduct;
                }
                if (orderDetail.DetailStyle == 2)
                {
                    orderDetailEntity.IdTour = orderDetail.IdProduct;
                }
                _OrderDetailRepository.Insert(orderDetailEntity);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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
                if (orderDetail.DetailStyle == 1)
                {
                    orderDetailEntity.IdRoom = orderDetail.IdProduct;
                }
                if (orderDetail.DetailStyle == 2)
                {
                    orderDetailEntity.IdTour = orderDetail.IdProduct;
                }
                _OrderDetailRepository.Insert(orderDetailEntity);

                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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
        public ResponseBase UpdateOrderDetail(InsertOrderDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var orderDetailEntity = _mapper.Map<InsertOrderDetailDto, OrderDetail>(orderDetail);
                if (orderDetail.DetailStyle == 1)
                {
                    orderDetailEntity.IdRoom = orderDetail.IdProduct;
                }
                if (orderDetail.DetailStyle == 2)
                {
                    orderDetailEntity.IdTour = orderDetail.IdProduct;
                }
                _OrderDetailRepository.Update(orderDetailEntity);

                var oder = _OrderRepository.GetById(orderDetail.IdOrder);
                oder.DateUpdate = DateTime.Now;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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
                orderDetail.IsDeleted = true;
                var oder = _OrderRepository.GetById(orderDetail.IdOrder);
                oder.DateUpdate = DateTime.Now;
                _OrderDetailRepository.Update(orderDetail);
                _OrderRepository.Update(oder);
                orderDetail.IsDeleted = true;
                _OrderDetailRepository.Update(orderDetail);

                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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
        public ResponseBase UpdateStatusOrder(byte Status, int IdOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                oder.Status = Status;
                _OrderRepository.Update(oder);

                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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
        public ResponseBase UpdatePTTTOrder(byte IdPTThanhtoan, int IdOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                oder.IdPaymentMethod = IdPTThanhtoan;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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

        public ResponseBase UpdateMoreInfoOrder(string moreinfo, int IdOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                oder.MoreInfo = moreinfo;
                _OrderRepository.Update(oder);
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
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

        public ResponseBase GetOrderDetailbyOrder(int IdOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listtemp = _OrderRepository.FindAll(x => x.Id == IdOrder).Include(x => x.OrderDetails).FirstOrDefault().OrderDetails.ToList();
                var list = _mapper.Map<List<OrderDetail>,List<OrderDetailShowDto>>(listtemp);
                
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
            IHotelFunction hotelFunction = new HotelFunction(_mapper);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrderInfo = _OrderRepository.FindAll(x => x.IdUser == IDUser).Include(x=>x.OrderDetails).Include(x=>x.IdPaymentMethodNavigation)
                    .Include(x=>x.StatusNavigation).Include(x=>x.IdUserNavigation);
                List<int> listIdOrder = new List<int>();
                List<OrderDetailInfoDto> listOrderDetailInfo = new List<OrderDetailInfoDto>();
                //foreach (var item in listOrderInfo)
                //{
                //    listIdOrder.Add(item.Id);
                //}
                //foreach (var i in listIdOrder)
                //{
                //    listOrderDetailInfo.Add(_OrderDetailRepository.FindAll(x => x.IdOrder == hotelId)
                //                            .Include(x => x.RoomMamenitis)
                //                            .Include(x => x.ViewDirectionNavigation)
                //                            .Include(x => x.Pictures.Take(4)).ToList();
                //}

                //OrderGetInfoDto hotelDetailDto = hotelFunction.CreateHotelDetailDto(hotel);


                //responseBase.Data = hotelDetailDto;
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
