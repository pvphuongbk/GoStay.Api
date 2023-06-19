using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.Data.Base;
using ResponseBase = GoStay.Data.Base.ResponseBase;
using ErrorCodeMessage = GoStay.Data.Base.ErrorCodeMessage;
using AutoMapper;
using GoStay.Data.OrderDto;
using Microsoft.EntityFrameworkCore;
using GoStay.Common.Helpers.Order;
using GoStay.DataDto.OrderDto;
using GoStay.Repository.Repositories;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using GoStay.Common.Extention;
using GoStay.Data.ServiceDto;
using GoStay.Data.TourDto;

namespace GoStay.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICommonRepository<Order> _OrderRepository;
        private readonly ICommonRepository<OrderDetail> _OrderDetailRepository;
        private readonly ICommonRepository<Tour> _tourRepository;
        private readonly ICommonRepository<HotelRoom> _roomRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Picture> _pictureRepository;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;
        private readonly IOrderFunction _orderFunction;


        public OrderService(ICommonRepository<Order> OrderRepository, ICommonRepository<OrderDetail> OrderRoomRepository, ICommonUoW commonUoW,
            IMapper mapper, ICommonRepository<Tour> tourRepository, ICommonRepository<HotelRoom> roomRepository,
            ICommonRepository<Hotel> hotelRepository, ICommonRepository<Picture> pictureRepository,
            ICommonRepository<User> userRepository, IOrderFunction orderFunction)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderRepository = OrderRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _tourRepository = tourRepository;
            _roomRepository = roomRepository;
            _pictureRepository = pictureRepository;
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _orderFunction = orderFunction;
        }

        public ResponseBase CreateOrder(OrderDto order)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (order.Style == 1)
                {
                    var room = _roomRepository.FindAll(x => x.Id == order.IdRoom).SingleOrDefault();
                    order.Price = room.CurrentPrice;
                    order.Discount = room.Discount;
                    order.TotalAmount = room.CurrentPrice * order.NumRoom * order.NumNight * (decimal)(100 - room.Discount) / 100;
                    order.IdHotel = room.Idhotel;
                }
                if (order.Style == 2)
                {
                    var tour = _tourRepository.FindAll(x => x.Id == order.IdTour).SingleOrDefault();
                    order.Price = (decimal)(tour.Price * order.Adult + tour.PriceChild * order.Children);
                    order.Discount = tour.Discount;
                    order.TotalAmount = order.Price * (decimal)(100 - order.Discount) / 100;
                }
                var ordercheck = _mapper.Map<OrderDto, Order>(order);
                ordercheck.IdPaymentMethod = order.IdPtthanhToan;
                ordercheck.DateCreate = DateTime.Now;
                _commonUoW.BeginTransaction();

                _OrderRepository.Insert(ordercheck);

                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = GetOrderbyId(ordercheck.Id).Data;
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
        public ResponseBase UpdateOrder(OrderDto order)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {

                var ordercheck = _OrderRepository.FindAll(x => x.Id == order.Id).SingleOrDefault();
                if (order.Style == 1)
                {
                    var room = _roomRepository.FindAll(x => x.Id == order.IdRoom).SingleOrDefault();
                    order.Price = room.CurrentPrice;
                    order.Discount = room.Discount;
                    order.TotalAmount = room.CurrentPrice * order.NumRoom * order.NumNight * (decimal)(100 - room.Discount) / 100;
                    order.IdHotel = room.Idhotel;
                    order.Status = 1;
                }
                if (order.Style == 2)
                {
                    var tour = _tourRepository.FindAll(x => x.Id == order.IdTour).SingleOrDefault();
                    order.Price = (decimal)(tour.Price * order.Adult + tour.PriceChild * order.Children);
                    order.Discount = tour.Discount;
                    order.TotalAmount = order.Price * (decimal)(100 - order.Discount) / 100;
                    order.Status = 1;
                }
                ordercheck = _mapper.Map<OrderDto, Order>(order);
                ordercheck.DateUpdate = DateTime.Now;
                _commonUoW.BeginTransaction();

                _OrderRepository.Update(ordercheck);
                _commonUoW.Commit();

                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = GetOrderbyId(ordercheck.Id).Data;


                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                responseBase.Data = null;
                return responseBase;
            }
        }

        public ResponseBase CheckOrder(OrderDto order)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (order.Style == 1)
                {
                    responseBase = CheckOrderHotel(order);
                }
                if (order.Style == 2)
                {
                    responseBase = CheckOrderTour(order);

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
        public ResponseBase CheckOrderHotel(OrderDto order)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                Order ordercheck = null;
                if (order.IdUser > 1)
                {
                    ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdRoom == order.IdRoom )
                        .FirstOrDefault();
                }
                else
                {
                    ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdRoom == order.IdRoom && x.Session == order.Session)
                        .FirstOrDefault();
                }

                if (ordercheck is null)
                {
                    if (order.IdUser > 1)
                    {
                        var ordercheck2 = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdHotel == order.IdHotel && x.Status<3)
                            .FirstOrDefault();
                        if (ordercheck2!=null)
                        {
                            responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                            responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                            responseBase.Data = UpdateOrder(order).Data;
                        }
                    } 
                    responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                    responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                    responseBase.Data = CreateOrder(order).Data;
                }
                else
                {
                    if (ordercheck.Status >= 3)
                    {
                        responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                        responseBase.Data = CreateOrder(order).Data;
                        return responseBase;
                    }
                    else
                    {
                        responseBase.Code = CheckOrderCodeMessage.GetOldOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.GetOldOrder.Value;
                        responseBase.Data = GetOrderbyId(ordercheck.Id).Data;
                        return responseBase;
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

        public ResponseBase CheckOrderTour(OrderDto order)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                Order ordercheck = null;
                
                if (order.IdUser > 1)
                {
                    ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdTour== order.IdTour)
                    .Include(x => x.OrderDetails)
                    .SingleOrDefault();
                }
                else
                {
                    ordercheck = _OrderRepository.FindAll(x => x.IdUser == order.IdUser && x.IdTour == order.IdTour && x.Session == order.Session)
                    .Include(x => x.OrderDetails)
                    .SingleOrDefault();
                }


                if (ordercheck is null)
                {
                    responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                    responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                    responseBase.Data = CreateOrder(order).Data;
                }
                else
                {
                    if (ordercheck.Status < 3)
                    {
                        responseBase.Code = CheckOrderCodeMessage.GetOldOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.GetOldOrder.Value;
                        responseBase.Data = GetOrderbyId(ordercheck.Id).Data;
                        return responseBase;
                    }
                    else
                    {
                        responseBase.Code = CheckOrderCodeMessage.GetOldOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.GetOldOrder.Value;
                        responseBase.Data = CreateOrder(order).Data;
                        return responseBase;
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
                    var room = _roomRepository.GetById(orderDetail.IdProduct);
                    orderDetailEntity.Price = (decimal)room.CurrentPrice;
                    orderDetailEntity.Discount = room.Discount;
                }
                if (orderDetail.DetailStyle == 2)
                {
                    orderDetailEntity.IdTour = orderDetail.IdProduct;
                    var tour = _tourRepository.GetById(orderDetail.IdProduct);
                    orderDetailEntity.Price = (decimal)tour.Price;
                    orderDetailEntity.Discount = tour.Discount;
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
        public ResponseBase UpdatePrePayment(decimal prepayment, int IdOrder)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var oder = _OrderRepository.GetById(IdOrder);
                oder.DateUpdate = DateTime.Now;
                oder.Prepayment = prepayment;
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
        public ResponseBase UpdateTotalAmount(UpdateTotalAmountOrderParam param)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var order = _OrderRepository.GetById(param.IdOrder);
                order.DateUpdate = DateTime.Now;
                order.Adult = param.Adult;
                order.Children = param.Children;
                order.Infant = param.Infant;
                order.TotalAmount = param.totalAmount;
                _OrderRepository.Update(order);

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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _pictureRepository, _userRepository);

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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository,_pictureRepository, _userRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var listOrder = _OrderRepository.FindAll(x => x.IdUser == IDUser)?
                                    .Include(x=>x.OrderDetails)
                                    .Include(x=>x.IdPaymentMethodNavigation)
                                    .Include(x=>x.StatusNavigation)
                                    .Include(x=>x.IdUserNavigation).ToList();
                var listOrderDetail = new List<OrderDetail>();
                if (listOrder == null)
                {
                    responseBase.Data = listOrderDetail;
                    return responseBase;
                }
                foreach ( var item in listOrder.Select(x=>x.Id))
                {
                    listOrderDetail.AddRange(_OrderDetailRepository.GetMany(x => x.IdOrder == item)
                        .Include(x=>x.IdRoomNavigation).ThenInclude(x=>x.PalletbedNavigation)
                        .Include(x => x.IdRoomNavigation).ThenInclude(x => x.RoomMamenitis).ThenInclude(x=>x.IdservicesNavigation)
                        .Include(x=>x.IdTourNavigation)
                        .ThenInclude(x=>x.TourDetails).ThenInclude(x=>x.IdStyleNavigation)
                        .Include(x => x.IdTourNavigation).ThenInclude(x=>x.IdTourStyleNavigation)
                        .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdTourTopicNavigation)
                        .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdDistrictFromNavigation).ThenInclude(x=>x.IdTinhThanhNavigation)
                        .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdStartTimeNavigation)
                        .Include(x => x.IdTourNavigation).ThenInclude(x => x.TourDistrictTos).ThenInclude(x => x.IdDistrictToNavigation).ThenInclude(x=>x.IdTinhThanhNavigation)
                        );
                }

                var listOrderInfo = new List<OrderGetInfoDto>();
                for (int i=0;i<listOrder.Count();i++)
                {
                    listOrderInfo.Add(_mapper.Map<Order, OrderGetInfoDto>(listOrder[i]));
                    var listdetail = listOrderDetail.Where(x => x.IdOrder == listOrder[i].Id).ToList();

                    for (int j = 0;j< listdetail.Count();j++)
                    {
                        //listOrderInfo[i].ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listdetail[j]));
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository,  _pictureRepository
               , _userRepository);
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
                    var listdetail = listOrderDetail.Where(x => x.IdOrder == listOrder[i].Id).ToList();

                    for (int j = 0; j < listdetail.Count(); j++)
                    {
                        //listOrderInfo[i].ListOrderDetails.Add(orderFunction.CreateOrderDetailInfoDto(listdetail[j]));
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
                        //orderInfor.ListOrderDetails.Add(_orderFunction.CreateOrderDetailInfoDto(detail));
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
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _pictureRepository, _userRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var orderInfo = new OrderGetInfoDto();
                var listOrderDetail = new List<OrderDetail>();

                var order = _OrderRepository.FindAll(x => x.Id == Id)
                    //room
                    .Include(x=>x.IdRoomNavigation)
                            .ThenInclude(x=>x.RoomViews).ThenInclude(x=>x.IdViewNavigation)
                    .Include(x => x.IdRoomNavigation)
                            .ThenInclude(x => x.RoomMamenitis).ThenInclude(x => x.IdservicesNavigation)
                    .Include(x => x.IdRoomNavigation)
                            .ThenInclude(x => x.PalletbedNavigation)
                    //tour
                    .Include(x=>x.IdTourNavigation).ThenInclude(x=>x.TourDistrictTos)
                        .ThenInclude(x=>x.IdDistrictToNavigation).ThenInclude(x=>x.IdTinhThanhNavigation)
                    .Include(x=>x.IdTourNavigation).ThenInclude(x=>x.IdTourStyleNavigation)
                    .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdTourTopicNavigation)
                    .Include(x => x.IdTourNavigation).ThenInclude(x => x.TourDetails)
                    .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdDistrictFromNavigation)
                        .ThenInclude(x=>x.IdTinhThanhNavigation)
                    .Include(x => x.IdTourNavigation).ThenInclude(x => x.IdStartTimeNavigation)
                    //exception
                    .Include(x => x.IdPaymentMethodNavigation)
                    .Include(x => x.StatusNavigation)
                    .Include(x => x.IdUserNavigation)
                    .SingleOrDefault();


                if (order == null)
                {
                    responseBase.Data = orderInfo;
                    return responseBase;
                }    

                orderInfo = _mapper.Map<Order, OrderGetInfoDto>(order);
                orderInfo.PaymentMethod = order.IdPaymentMethodNavigation.PhuongThuc;

                if(orderInfo.Style==1)
                {
                    var room = _roomRepository.FindAll(x => x.Id == order.IdRoom).SingleOrDefault();
                    orderInfo.Room = CreateHotelRoomOrderDto(room);
                    orderInfo.Room.NumNight = order.NumNight;
                    orderInfo.Room.NumRoom = order.NumRoom;

                }
                if (orderInfo.Style == 2)
                {
                    var tour = _tourRepository.FindAll(x => x.Id == order.IdTour).SingleOrDefault();
                    orderInfo.Tour = CreateTourOrderDto(tour);
                    orderInfo.Tour.Adult = (int)order.Adult;
                    orderInfo.Tour.Children = (int)order.Children;
                    orderInfo.Tour.Infant = (int)order.Infant;

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
                    var xxxx = orders.Where(x => x.Date.Day == i.Day);
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

        public ResponseBase GetListOrderSearch(OrderSearchParam param)
        {
            ResponseBase response = new ResponseBase();
            var Data = OrderRepository.SearchListOrder(param);
            Data.ForEach(x => x.Slug = x.HotelName?.RemoveUnicode().Replace(" ", "-").Replace(",", string.Empty)
                            .Replace(".", "-")
                            .Replace("/", "-").Replace("--", string.Empty)
                            .Replace("\"", string.Empty).Replace("\'", string.Empty)
                            .Replace("(", string.Empty).Replace(")", string.Empty)
                            .Replace("*", string.Empty).Replace("%", string.Empty)
                            .Replace("&", "-").Replace("@", string.Empty).ToLower());
            response.Data = Data;
            return response;
        }

        public ResponseBase DeleteRoomInOrder(int IdDetail, int IdOrder)
        {
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _pictureRepository, _userRepository);
            ResponseBase response = new ResponseBase();
            try
            {
                var orders = _OrderRepository.FindAll(x => x.Id == IdOrder && x.OrderDetails.Any(z=>z.Id== IdDetail)).Include(x => x.OrderDetails);
                if(orders.Count()<=0)
                {
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    response.Message = ErrorCodeMessage.NotFound.Value;

                    response.Data = "Not Found";
                    return response;

                }
                if (orders.Count() > 1)
                {
                    response.Code = ErrorCodeMessage.Exception.Key;
                    response.Message = ErrorCodeMessage.Exception.Value;

                    response.Data = "exception";
                    return response;

                }
                _commonUoW.BeginTransaction();
                _OrderDetailRepository.Remove(IdDetail);
                _commonUoW.Commit();

                response.Data = "Success";
                return response;
            }
            catch
            {
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = ErrorCodeMessage.Exception.Value;

                response.Data = "exception";
                return response;

            }
            
        }
        public ResponseBase GetRoomInOrder(int Id)
        {
            IOrderFunction orderFunction = new OrderFunction(_mapper, _hotelRepository, _pictureRepository, _userRepository);
            ResponseBase responseBase = new ResponseBase();
            try
            {

                var room = _roomRepository.FindAll(x=>x.Id == Id).SingleOrDefault();


                if (room == null)
                {
                    responseBase.Data = null;
                    return responseBase;
                }

                var roomDto = CreateHotelRoomOrderDto(room);

                responseBase.Data = roomDto;
                return responseBase;
            }
            catch (Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }



        public RoomOrderDto CreateHotelRoomOrderDto(HotelRoom roomOrderDetail)
        {
            var hotelRoomOrderDto = _mapper.Map<HotelRoom, RoomOrderDto>(roomOrderDetail);
            var hotel = _hotelRepository.GetById(roomOrderDetail.Idhotel);
            hotelRoomOrderDto.HotelName = hotel.Name;
            hotelRoomOrderDto.RoomName = roomOrderDetail.Name;

            hotelRoomOrderDto.Address = hotel.Address;
            hotelRoomOrderDto.Rating = hotel.Rating;
            hotelRoomOrderDto.ReviewScore = (int?)hotel.ReviewScore;
            hotelRoomOrderDto.NumberReviewers = hotel.NumberReviewers;
            if (roomOrderDetail.RoomViews != null && roomOrderDetail.RoomViews.Count() > 0)
            {
                hotelRoomOrderDto.ViewDirection = roomOrderDetail.RoomViews.FirstOrDefault().IdViewNavigation.ViewDirection1;
            }
            hotelRoomOrderDto.Pictures = _pictureRepository.FindAll(x => x.HotelRoomId == roomOrderDetail.Id && x.Type == 1)?.Select(x => x.Url).Take(1).ToList();
            hotelRoomOrderDto.Pictures.AddRange(_pictureRepository.FindAll(x => x.HotelId == hotel.Id && x.Type == 0)?.Select(x => x.Url).Take(1).ToList());
            hotelRoomOrderDto.PalletbedText = roomOrderDetail.PalletbedNavigation?.Text;

            hotelRoomOrderDto.Services = _mapper.Map<List<Service>, List<ServiceDetailHotelDto>>
                                        (roomOrderDetail.RoomMamenitis.Select(x => x.IdservicesNavigation).ToList());

            return hotelRoomOrderDto;
        }
        public TourOrderDto CreateTourOrderDto(Tour tourOrderDetail)
        {
            var tourOrderDto = _mapper.Map<Tour, TourOrderDto>(tourOrderDetail);
            tourOrderDto.TourStyle = tourOrderDetail.IdTourStyleNavigation.TourStyle1;
            tourOrderDto.TourTopic = tourOrderDetail.IdTourTopicNavigation.TourTopic1;
            tourOrderDto.UserName = _userRepository.GetById(tourOrderDto.IdUser)?.UserName;
            tourOrderDto.ProvinceFrom = tourOrderDetail.IdDistrictFromNavigation.IdTinhThanhNavigation.TenTt;
            if (tourOrderDetail.IdStartTimeNavigation != null)
            {
                tourOrderDto.StartTime = tourOrderDetail.IdStartTimeNavigation.StartDate;
            }
            tourOrderDto.Pictures = _pictureRepository.FindAll(x => x.TourId == tourOrderDetail.Id && x.Type == 2)?.Select(x => x.Url).Take(2).ToList();
            var listTourDetail = tourOrderDetail.TourDetails.ToList();

            tourOrderDto.TourDetails = _mapper.Map<List<TourDetail>, List<TourDetailDto>>(listTourDetail);
            var listprovinceto = new List<string>();
            foreach (var item in tourOrderDetail.TourDistrictTos.Select(x => x.IdDistrictToNavigation.IdTinhThanhNavigation.TenTt))
            {
                listprovinceto.Add(item);
            }
            tourOrderDto.ProvinceTo = listprovinceto;
            return tourOrderDto;
        }
        //public ResponseBase GetBookedDateRoom(int IdRoom)
        //{
        //    ResponseBase responseBase = new ResponseBase();
        //    try
        //    {
        //        var orders = _OrderRepository.FindAll(x=>x.check)
        //        responseBase.Data = orderInfo.ListOrderDetails;
        //        return responseBase;
        //    }
        //    catch (Exception e)
        //    {
        //        responseBase.Code = ErrorCodeMessage.Exception.Key;
        //        responseBase.Message = e.Message;
        //        return responseBase;
        //    }
        //}
    }
}
