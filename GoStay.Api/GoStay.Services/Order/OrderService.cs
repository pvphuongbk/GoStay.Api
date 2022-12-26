﻿using GoStay.Common;
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
using System.Runtime.CompilerServices;
using GoStay.Common.Helpers.Order;

namespace GoStay.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICommonRepository<Order> _OrderRepository;
        private readonly ICommonRepository<OrderDetail> _OrderDetailRepository;
        private readonly ICommonRepository<Tour> _tourRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Service> _serviceRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<ViewDirection> _viewRepository;

        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;


        public OrderService(ICommonRepository<Order> OrderRepository,ICommonRepository<OrderDetail> OrderRoomRepository,ICommonUoW commonUoW,
            IMapper mapper, ICommonRepository<Tour> tourRepository, ICommonRepository<HotelRoom> roomRepository, ICommonRepository<Hotel> hotelRepository
            , ICommonRepository<Service> serviceRepository, ICommonRepository<Picture> pictureRepository, ICommonRepository<ViewDirection> viewRepository)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderRepository = OrderRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _tourRepository = tourRepository;
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
            _pictureRepository = pictureRepository;
            _viewRepository = viewRepository;
            _hotelRepository = hotelRepository;

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
                responseBase.Data = GetOrderbyId(orderEntity.Id).Data;
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

        public ResponseBase CheckOrder(OrderDto order, OrderDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {

                var ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdHotel == order.IdHotel)
                    .Include(x=>x.OrderDetails)
                    .SingleOrDefault();

                if (ordercheck is null)
                {
                    responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                    responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                    responseBase.Data = CreateOrder(order, orderDetail).Data;
                }
                else
                {
                    int addDetail = 0;
                    foreach (var item in ordercheck.OrderDetails)
                    {
                        if (item.IdRoom == orderDetail.IdProduct)
                        {
                            addDetail ++;
                            if (ordercheck.Status == 3)
                            {
                                responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                                responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                                responseBase.Data = CreateOrder(order, orderDetail).Data;
                            }
                            else
                            {
                                responseBase.Code = CheckOrderCodeMessage.GetOldOrder.Key;
                                responseBase.Message = CheckOrderCodeMessage.GetOldOrder.Value;
                                responseBase.Data = GetOrderbyId(ordercheck.Id).Data;
                            }
                        }

                    }
                    if (addDetail == 0)
                    {
                        responseBase.Code = CheckOrderCodeMessage.CreateNewDetail.Key;
                        responseBase.Message = CheckOrderCodeMessage.CreateNewDetail.Value;
                        responseBase.Data = AddOrderDetail(ordercheck.Id, orderDetail).Data;
                    }

                }
                return responseBase;
            }
            catch (Exception e)
            {
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
                responseBase.Data = GetOrderbyId(IdOrder).Data;
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository,_serviceRepository,_pictureRepository,_viewRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.IdUser == IDUser).Include(x=>x.OrderDetails).Include(x=>x.IdPaymentMethodNavigation)
                    .Include(x=>x.StatusNavigation).Include(x=>x.IdUserNavigation).ToList();
                var listOrderDetail = new List<OrderDetail>();
                foreach( var item in listOrder.Select(x=>x.Id))
                {
                    listOrderDetail.AddRange(_OrderDetailRepository.GetMany(x => x.IdOrder == item).Include(x=>x.IdRoomNavigation).Include(x=>x.IdTourNavigation));
                }

                var listOrderInfo = new List<OrderGetInfoDto>();
                for (int i=0;i<listOrder.Count();i++)
                {
                    listOrderInfo.Add(_mapper.Map<Order, OrderGetInfoDto>(listOrder[i]));
                    //var listdetail = listOrder[i].OrderDetails.ToList();
                    var listdetail = listOrderDetail.Where(x => x.IdOrder == listOrder[i].Id).ToList();

                    for (int j = 0;j< listdetail.Count();j++)
                    {
                        listOrderInfo[i].ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listdetail[j]));
                    }
                }

                responseBase.Data = listOrderInfo;
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _serviceRepository, _pictureRepository, _viewRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.Session == session).Include(x => x.OrderDetails).Include(x => x.IdPaymentMethodNavigation)
                    .Include(x => x.StatusNavigation).Include(x => x.IdUserNavigation).ToList();
                var listOrderDetail = new List<OrderDetail>();
                foreach (var item in listOrder.Select(x => x.Id))
                {
                    listOrderDetail.AddRange(_OrderDetailRepository.GetMany(x => x.IdOrder == item).Include(x => x.IdRoomNavigation).Include(x => x.IdTourNavigation));
                }

                var listOrderInfo = new List<OrderGetInfoDto>();
                for (int i = 0; i < listOrder.Count(); i++)
                {
                    listOrderInfo.Add(_mapper.Map<Order, OrderGetInfoDto>(listOrder[i]));
                    //var listdetail = listOrder[i].OrderDetails.ToList();
                    var listdetail = listOrderDetail.Where(x => x.IdOrder == listOrder[i].Id).ToList();

                    for (int j = 0; j < listdetail.Count(); j++)
                    {
                        listOrderInfo[i].ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listdetail[j]));
                    }
                }

                responseBase.Data = listOrderInfo;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }



        public ResponseBase GetOrderbyId(int Id)
        {
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _serviceRepository, _pictureRepository, _viewRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var order = _OrderRepository.FindAll(x => x.Id == Id).Include(x => x.OrderDetails).Include(x => x.IdPaymentMethodNavigation)
                    .Include(x => x.StatusNavigation).Include(x => x.IdUserNavigation).SingleOrDefault();
                var listOrderDetail = new List<OrderDetail>();

                listOrderDetail.AddRange(_OrderDetailRepository.GetMany(x => x.IdOrder == Id).Include(x => x.IdRoomNavigation).Include(x => x.IdTourNavigation));
                

                var orderInfo = new OrderGetInfoDto();

                orderInfo = _mapper.Map<Order, OrderGetInfoDto>(order);
                var listdetail = listOrderDetail;

                for (int j = 0; j < listdetail.Count(); j++)
                {
                    orderInfo.ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listdetail[j]));
                }
                

                responseBase.Data = orderInfo;
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
