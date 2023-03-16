using AutoMapper;
using GoStay.Data.Base;
using GoStay.Data.Ticket;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace GoStay.Services.OrderTickets
{
    public class OrderTicketService : IOrderTicketService
    {
        private readonly ICommonRepository<OrderTicket> _OrderTicketRepository;
        private readonly ICommonRepository<OrderTicketDetail> _OrderDetailRepository;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonRepository<TicketPassenger> _passengerRepository;

        private readonly ICommonUoW _commonUoW;
        private readonly IMapper _mapper;


        public OrderTicketService(ICommonRepository<OrderTicket> OrderRepository, ICommonRepository<OrderTicketDetail> OrderRoomRepository,
            ICommonUoW commonUoW, ICommonRepository<TicketPassenger> passengerRepository,
            IMapper mapper, ICommonRepository<User> userRepository)
        {
            _OrderDetailRepository = OrderRoomRepository;
            _OrderTicketRepository = OrderRepository;
            _commonUoW = commonUoW;
            _mapper = mapper;
            _userRepository = userRepository;
            _passengerRepository = passengerRepository;
        }
        public ResponseBase GetAllOrderTicket(int pageIndex, int pageSize)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                List<OrderTicketAdminDto> ListData = new List<OrderTicketAdminDto>();
                var listOrder = _OrderTicketRepository.FindAll()
                                .Include(x => x.StatusNavigation)
                                .Include(x => x.OrderTicketDetails).ThenInclude(x => x.TicketPassengers)
                                .Include(x => x.IdUserNavigation)
                                .Include(x => x.IdPtthanhToanNavigation)
                                .OrderBy(x => x.DateCreate);
                var count = listOrder.Count();
                int take = pageSize;
                int skip = pageSize*(pageIndex-1);

                if (pageIndex*pageSize > count)
                {
                    take = pageIndex * pageSize - count;
                }
                var orders = listOrder.Skip(skip).Take(take);
                ListData = _mapper.Map<List<OrderTicket>,List<OrderTicketAdminDto>>(orders.ToList());
                ListData.ForEach(x => x.TicketDetail = _mapper.Map<OrderTicketDetail, OrderTicketDetailAdminDto>
                                (orders.Where(z => z.Id == x.Id).SingleOrDefault().OrderTicketDetails.SingleOrDefault()));
                responseBase.Data = ListData;
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                return responseBase;
            }
            catch(Exception e)
            {
                responseBase.Code = ErrorCodeMessage.Exception.Key;
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase CreateOrderTicket(OrderTicketDto order, OrderTicketDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                _commonUoW.BeginTransaction();
                var orderTicketEntity = _mapper.Map<OrderTicketDto, OrderTicket>(order);
                _OrderTicketRepository.Insert(orderTicketEntity);
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                orderDetail.IdOrder = orderTicketEntity.Id;
                var orderDetailEntity = _mapper.Map<OrderTicketDetailDto, OrderTicketDetail>(orderDetail);
                orderDetailEntity.DepartureDate = DateTime.ParseExact(orderDetail.DepartureDateText, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                orderDetailEntity.StartDate = DateTime.ParseExact(orderDetail.StartDateText, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                orderDetailEntity.EndDate = DateTime.ParseExact(orderDetail.EndDateText, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);


                decimal Price = 0;
                foreach (var passenger in orderDetail.Passengers)
                {
                    Price = Price + passenger.Price + (decimal)orderDetail.ServiceFee + (decimal)orderDetail.IssueFee;
                }
                orderDetailEntity.Price = Price;
                _OrderDetailRepository.Insert(orderDetailEntity);
                _commonUoW.Commit();

                _commonUoW.BeginTransaction();
                foreach (var passenger in orderDetail.Passengers)
                {
                    var passengerEntity = _mapper.Map<TicketPassengerDto, TicketPassenger>(passenger);
                    passengerEntity.IdTicket = orderDetailEntity.Id;
                    _passengerRepository.Insert(passengerEntity);
                }
                _commonUoW.Commit();
                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = GetOrderTicketbyId(orderTicketEntity.Id).Data;
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

        public ResponseBase CheckOrderTicket(OrderTicketDto order, OrderTicketDetailDto orderDetail)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                OrderTicket ordercheck = null;

                ordercheck = _OrderTicketRepository.FindAll(x => x.IdUser == order.IdUser && x.DataFlightSession == order.DataFlightSession
                                && x.FlightSession == order.FlightSession && x.Status != 3)
                                .Include(x => x.OrderTicketDetails).ThenInclude(x => x.TicketPassengers).SingleOrDefault();

                if (ordercheck is null)
                {
                    responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                    responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                    responseBase.Data = CreateOrderTicket(order, orderDetail).Data;
                }
                else
                {
                    if (ordercheck.Status == 3)
                    {
                        responseBase.Code = CheckOrderCodeMessage.CreateNewOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.CreateNewOrder.Value;
                        responseBase.Data = CreateOrderTicket(order, orderDetail).Data;
                    }
                    else
                    {
                        responseBase.Code = CheckOrderCodeMessage.GetOldOrder.Key;
                        responseBase.Message = CheckOrderCodeMessage.GetOldOrder.Value;
                        responseBase.Data = GetOrderTicketbyId(ordercheck.Id).Data;
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

        public ResponseBase GetOrderTicketbyId(int Id)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var orderTicketShow = new OrderTicketShowDto();


                var order = _OrderTicketRepository.FindAll(x => x.Id == Id)?.
                            Include(x => x.StatusNavigation).
                            Include(x => x.IdPtthanhToanNavigation).
                            Include(x => x.OrderTicketDetails).ThenInclude(x => x.TicketPassengers)
                            .SingleOrDefault();

                if (order == null)
                {
                    responseBase.Data = orderTicketShow;
                    return responseBase;
                }

                var ticketDetail = order.OrderTicketDetails.FirstOrDefault();
                var lisspassenger = ticketDetail.TicketPassengers;

                orderTicketShow = _mapper.Map<OrderTicket, OrderTicketShowDto>(order);
                orderTicketShow.StatusText = order.StatusNavigation.Status;
                orderTicketShow.Paymentmethod = order.IdPtthanhToanNavigation.PhuongThuc;
                orderTicketShow.DateCreateText = order.DateCreate.ToString("dd/MM/yyyy hh:mm");
                orderTicketShow.TicketDetail = _mapper.Map<OrderTicketDetail, OrderTicketDetailShowDto>(ticketDetail);
                orderTicketShow.TicketDetail.DepartureDateText = ticketDetail.DepartureDate.ToString("dd/MM/yyyy");
                orderTicketShow.TicketDetail.StartDateText = ticketDetail.StartDate.ToString("dd/MM/yyyy hh:mm");
                orderTicketShow.TicketDetail.EndDateText = ticketDetail.EndDate.ToString("dd/MM/yyyy hh:mm");
                orderTicketShow.TicketDetail.Passengers = new List<TicketPassengerShowDto>();
                foreach (var passenger in lisspassenger)
                {
                    var item = _mapper.Map<TicketPassenger, TicketPassengerShowDto>(passenger);
                    orderTicketShow.TicketDetail.Passengers.Add(item);
                }

                responseBase.Data = orderTicketShow;
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
