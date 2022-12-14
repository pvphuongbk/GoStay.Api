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
using System.Runtime.CompilerServices;
using GoStay.Common.Helpers.Order;
using System.Linq.Expressions;
using GoStay.DataDto.OrderDto;

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
        private readonly ICommonRepository<Palletbed> _palletbedRepository;

        private readonly ICommonRepository<TourStyle> _tourStyleRepository;
        private readonly ICommonRepository<TourTopic> _tourTopicRepository;
        private readonly ICommonRepository<TourDetail> _tourDetailRepository;
        private readonly ICommonRepository<TinhThanh> _tinhThanhRepository;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonRepository<TourDistrictTo> _tourProvinceToRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;
        private readonly IOrderFunction _orderFunction;


        public OrderService(ICommonRepository<Order> OrderRepository, ICommonRepository<OrderDetail> OrderRoomRepository, ICommonUoW commonUoW,
            IMapper mapper, ICommonRepository<Tour> tourRepository, ICommonRepository<HotelRoom> roomRepository,
            ICommonRepository<Hotel> hotelRepository, ICommonRepository<Service> serviceRepository,
            ICommonRepository<Picture> pictureRepository, ICommonRepository<ViewDirection> viewRepository,
            ICommonRepository<Palletbed> palletbedRepository, ICommonRepository<TourStyle> tourStyleRepository,
            ICommonRepository<TourTopic> tourTopicRepository, ICommonRepository<TourDetail> tourDetailRepository,
            ICommonRepository<TinhThanh> tinhThanhRepository, ICommonRepository<User> userRepository, ICommonRepository<TourDistrictTo> tourProvinceToRepository, IOrderFunction orderFunction)
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
            _palletbedRepository = palletbedRepository;
            _userRepository = userRepository;
            _tourStyleRepository = tourStyleRepository;
            _tourTopicRepository = tourTopicRepository;
            _tourDetailRepository = tourDetailRepository;
            _tinhThanhRepository = tinhThanhRepository;
            _tourProvinceToRepository = tourProvinceToRepository;
            _orderFunction = orderFunction;
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

                var ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdHotel == order.IdHotel&& x.Status!=3)
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
                        if (item.DetailStyle == 1)
                        {
                            if (item.IdRoom == orderDetail.IdProduct)
                            {
                                addDetail++;
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
                        if (item.DetailStyle == 2)
                        {
                            if (item.IdTour == orderDetail.IdProduct)
                            {
                                addDetail++;
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

        public ResponseBase UpdateUserIDbySession(int IdUser, string Session)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var listoder = _OrderRepository.FindAll(x=>x.Session == Session);
                if (listoder == null)
                {
                    _commonUoW.Commit();
                    responseBase.Code = ErrorCodeMessage.Exception.Key;
                    responseBase.Message = ErrorCodeMessage.Exception.Value;
                    return responseBase;
                }
                else
                {
                    foreach (var item in listoder)
                    {
                        item.DateUpdate = DateTime.Now;
                        item.IdUser = IdUser;
                        _OrderRepository.Update(item);
                    }
                    _commonUoW.Commit();
                    responseBase.Code = ErrorCodeMessage.Success.Key;
                    responseBase.Message = ErrorCodeMessage.Success.Value;
                    return responseBase;
                }
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _serviceRepository, _pictureRepository, _viewRepository,
                _palletbedRepository,_tourStyleRepository,_tourTopicRepository, _tinhThanhRepository, _userRepository,_tourDetailRepository,_tourProvinceToRepository);

            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listtemp = _OrderRepository.FindAll(x => x.Id == IdOrder)?.Include(x => x.OrderDetails).FirstOrDefault().OrderDetails.ToList();
                var list = new List<OrderDetailInfoDto>();
                if(listtemp == null)
                {
                    responseBase.Data = list;
                    return responseBase;
                }
                foreach (var item in listtemp)
                {
                    list.Add(orderFunction.CreateOrderDetailInfoDto(item));
                }    
                
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository,_serviceRepository,_pictureRepository,_viewRepository,
                _palletbedRepository,_tourStyleRepository, _tourTopicRepository, _tinhThanhRepository, _userRepository, _tourDetailRepository, _tourProvinceToRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.IdUser == IDUser)?.Include(x=>x.OrderDetails).Include(x=>x.IdPaymentMethodNavigation)
                    .Include(x=>x.StatusNavigation).Include(x=>x.IdUserNavigation).ToList();
                var listOrderDetail = new List<OrderDetail>();
                if (listOrder == null)
                {
                    responseBase.Data = listOrderDetail;
                    return responseBase;
                }
                foreach ( var item in listOrder.Select(x=>x.Id))
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _serviceRepository, _pictureRepository, _viewRepository, 
                _palletbedRepository, _tourStyleRepository, _tourTopicRepository, _tinhThanhRepository, _userRepository, _tourDetailRepository, _tourProvinceToRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.Session == session)?.Include(x => x.OrderDetails).Include(x => x.IdPaymentMethodNavigation)
                    .Include(x => x.StatusNavigation).Include(x => x.IdUserNavigation).ToList();
                var listOrderDetail = new List<OrderDetail>();
                if (listOrder == null)
                {
                    responseBase.Data = listOrderDetail;
                    return responseBase;
                }
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

        public ResponseBase GetOrderbySession2(string session)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.Session == session)
                                                    .Include(x => x.OrderDetails)
                                                        .ThenInclude(x => x.IdRoomNavigation)
                                                    .Include(x => x.OrderDetails)
                                                        .ThenInclude(x => x.IdTourNavigation)
                                                    .ToList();
                if (listOrder == null)
                {
                    responseBase.Data = new List<OrderDetail>();
                    return responseBase;
                }

                var listOrderInfo = _mapper.Map<List<OrderGetInfoDto>>(listOrder);
                foreach (var order in listOrder)
                {
                    var orderInfor = listOrderInfo.FirstOrDefault(x => x.Id == order.Id);
                    if (orderInfor == null)
                        continue;

                    foreach (var detail in order.OrderDetails)
                    {
                        orderInfor.ListOrderDetails.Add(_orderFunction.CreateOrderDetailInfoDto(detail));
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _serviceRepository, _pictureRepository, _viewRepository, 
                _palletbedRepository, _tourStyleRepository, _tourTopicRepository, _tinhThanhRepository, _userRepository, _tourDetailRepository, _tourProvinceToRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var orderInfo = new OrderGetInfoDto();
                var listOrderDetail = new List<OrderDetail>();

                var order = _OrderRepository.FindAll(x => x.Id == Id)?.Include(x => x.OrderDetails).Include(x => x.IdPaymentMethodNavigation)
                    .Include(x => x.StatusNavigation).Include(x => x.IdUserNavigation).SingleOrDefault();

                if (order == null)
                {
                    responseBase.Data = orderInfo;
                    return responseBase;
                }    

                listOrderDetail.AddRange(_OrderDetailRepository.GetMany(x => x.IdOrder == Id)?.
                    Include(x => x.IdRoomNavigation).Include(x => x.IdTourNavigation).OrderByDescending(x=>x.DateCreate));

                orderInfo = _mapper.Map<Order, OrderGetInfoDto>(order);
                orderInfo.Status =order.Status;
                orderInfo.StatusDetail = order.StatusNavigation.Status;
                orderInfo.PaymentMethod = order.IdPaymentMethodNavigation.PhuongThuc;

                for (int j = 0; j < listOrderDetail.Count(); j++)
                {
                    orderInfo.ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listOrderDetail[j]));
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

        private List<ChartOdertDetailValue> CreateChartOdertDetailValue(ICollection<OrderDetail> orderDetails)
        {
            return orderDetails.Select(x => new ChartOdertDetailValue
            {
                Discount = x.Discount,
                Price = x.Price
            }).ToList();
        }

        public ResponseBase GetOrderTotalMoneyByMonth(int month, int year, int status)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<ChartOdertByDayDto> chart = new List<ChartOdertByDayDto>();

                var orders = _OrderRepository.FindAll(x => !x.IsDeleted && x.DateCreate.Year == year && x.DateCreate.Month == month && (status == 0 || x.Status == status))
                                               .Include(x => x.OrderDetails)
                                               .ToList()
                                                .Select(x => new ChartOdertValue
                                                {
                                                    Date = x.DateCreate,
                                                    ID = x.Id,
                                                    ChartOdertDetailValues = CreateChartOdertDetailValue(x.OrderDetails)
                                                }).ToList();
                var minDate = new DateTime(year, month, 1);
                var endDate = minDate.AddMonths(1);
                var maxDate = endDate < DateTime.Now.Date ? endDate : DateTime.Now.Date.AddDays(1);
                for (DateTime i = minDate; i < maxDate; i = i.AddDays(1))
                {
                    var totalMoney = orders.Where(x => x.Date.Day == i.Day).SelectMany(x => x.ChartOdertDetailValues).Sum(x => x.ActualPrice);
                    chart.Add(new ChartOdertByDayDto
                    {
                        Value = (int)totalMoney,
                        Day = i.Day
                    });
                }
                responseBase.Data = chart;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetOrderByMonth(int month, int year, int status)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<ChartOdertByDayDto> chart = new List<ChartOdertByDayDto>();

                var orders = _OrderRepository.FindAll(x => !x.IsDeleted && x.DateCreate.Year == year && x.DateCreate.Month == month && (status == 0 || x.Status == status))
                                                .Select(x => new ChartOdertValue
                                                {
                                                    Date = x.DateCreate,
                                                    ID = x.Id,
                                                }).ToList();
                var minDate = new DateTime(year, month, 1);
                var endDate = minDate.AddMonths(1);
                var maxDate = endDate < DateTime.Now.Date ? endDate : DateTime.Now.Date.AddDays(1);
                for (DateTime i = minDate; i < maxDate; i = i.AddDays(1))
                {
                    var count = orders.Count(x => x.Date.Day == i.Day);
                    chart.Add(new ChartOdertByDayDto
                    {
                        Value = count,
                        Day = i.Day
                    });
                }
                responseBase.Data = chart;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetOrderRoomByMonth(int month, int year, int status)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<ChartOdertByDayDto> chart = new List<ChartOdertByDayDto>();

                var orders = _OrderRepository.FindAll(x => !x.IsDeleted && x.DateCreate.Year == year && x.DateCreate.Month == month && (status == 0 || x.Status == status))
                                               .Include(x => x.OrderDetails)
                                               .ToList()
                                                .Select(x => new ChartOdertValue
                                                {
                                                    Date = x.DateCreate,
                                                    ID = x.Id,
                                                    ChartOdertDetailValues = CreateChartOdertDetailValue(x.OrderDetails)
                                                }).ToList();
                var minDate = new DateTime(year, month, 1);
                var endDate = minDate.AddMonths(1);
                var maxDate = endDate < DateTime.Now.Date ? endDate : DateTime.Now.Date.AddDays(1);
                for (DateTime i = minDate; i < maxDate; i = i.AddDays(1))
                {
                    var countRoom = orders.Where(x => x.Date.Day == i.Day).SelectMany(x => x.ChartOdertDetailValues).Count();
                    chart.Add(new ChartOdertByDayDto
                    {
                        Value = countRoom,
                        Day = i.Day
                    });
                }
                responseBase.Data = chart;
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
