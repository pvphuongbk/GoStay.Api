using GoStay.Common.Extention;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataDto.RatingDto;
using GoStay.Repository.Repositories;
using GoStay.Services.Ratings;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace GoStay.Services.Reviews
{
    public class RatingService : IRatingService
    {
        private readonly ICommonRepository<HotelRating> _hotelRatingRepository;
        private readonly ICommonUoW _icommonUoWRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Order> _orderRepository;
        private readonly ICommonRepository<User> _userRepository;

        private readonly ICommonRepository<OrderDetail> _orderDetailRepository;

        public RatingService(ICommonRepository<HotelRating> hotelRatingRepository, ICommonUoW icommonUoWRepository,
            ICommonRepository<Hotel> hotelRepository, ICommonRepository<Order> orderRepository
            , ICommonRepository<OrderDetail> orderDetailRepository, ICommonRepository<User> userRepository)
        {
            _hotelRatingRepository = hotelRatingRepository;
            _icommonUoWRepository = icommonUoWRepository;
            _hotelRepository = hotelRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _userRepository = userRepository;
        }
        
        public ResponseBase GetRatingByUser(int hotelId, int userId)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == userId && x.IdHotel == hotelId).FirstOrDefault();
                if (exitsRating != null)
                {
                    var dto = new GetRatingDto
                    {
                        LocationScore = exitsRating.LocationScore,
                        ValueScore = exitsRating.ValueScore,
                        ServiceScore = exitsRating.ServiceScore,
                        CleanlinessScore = exitsRating.CleanlinessScore,
                        RoomsScore = exitsRating.RoomsScore,
                        Description = exitsRating.Description,
                    };

                    response.Data = dto;
                }

                return response;
            }
            catch (Exception e)
            {
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }
        }
        public ResponseBase GetRatingByHotel(int hotelId)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                var exitsRatings = _hotelRatingRepository.FindAll(x => x.IdHotel == hotelId)
                                                            .Include(x => x.IdUserNavigation)
                                                            .ToList();
                var res = new List<GetRatingByHotelDto>();
                foreach (var exitsRating in exitsRatings)
                {
                    if (exitsRating != null)
                    {
                        var dto = new GetRatingByHotelDto
                        {
                            UserEmail = exitsRating.IdUserNavigation?.Email,
                            UserId = exitsRating.IdUserNavigation?.UserId,
                            UserName = exitsRating.IdUserNavigation?.UserName,
                            LocationScore = exitsRating.LocationScore,
                            ValueScore = exitsRating.ValueScore,
                            ServiceScore = exitsRating.ServiceScore,
                            CleanlinessScore = exitsRating.CleanlinessScore,
                            RoomsScore = exitsRating.RoomsScore,
                            Description = exitsRating.Description,
                        };

                        res.Add(dto);
                    }
                }
                response.Data = res;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = ex.Message;
                return response;
            }
        }
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                _icommonUoWRepository.BeginTransaction();
                var (re, check) = UpdateScoreForHotel(dto);
                if (!check)
                {
                    response.Message = "User Id không tồn tại";
                    response.Code = ErrorCodeMessage.NotFound.Key;
                    return response;
                }

                var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser == dto.UserId && x.IdHotel == dto.HotelId).FirstOrDefault();
                if (exitsRating != null)
                {
                    exitsRating.LocationScore = dto.LocationScore;
                    exitsRating.ValueScore = dto.ValueScore;
                    exitsRating.ServiceScore = dto.ServiceScore;
                    exitsRating.CleanlinessScore = dto.CleanlinessScore;
                    exitsRating.RoomsScore = dto.RoomsScore;
                    exitsRating.Description = dto.Description;
                    exitsRating.DateUpdate = DateTime.Now;
                    _hotelRatingRepository.Update(exitsRating);
                }
                else
                {
                    var item = new HotelRating
                    {
                        IdHotel = dto.HotelId,
                        IdUser = dto.UserId,
                        Description = dto.Description,
                        ServiceScore = dto.ServiceScore,
                        CleanlinessScore = dto.CleanlinessScore,
                        LocationScore = dto.LocationScore,
                        RoomsScore = dto.RoomsScore,
                        ValueScore = dto.ValueScore,
                        DateReviews = DateTime.Now,
                    };
                    _hotelRatingRepository.Insert(item);
                }

                _icommonUoWRepository.Commit();
                response.Data = re;
                return response;
            }
            catch (Exception e)
            {
                response.Code = ErrorCodeMessage.Exception.Key;
                response.Message = e.Message;
                return response;
            }
        }
        public ResponseBase GetUserBoxReview(int idHotel, int idUser)
        {
            ResponseBase response = new ResponseBase();
            UserBoxReview userBoxReview = new UserBoxReview();
            try
            {
                var order = _orderRepository.FindAll(x => x.IdUser == idUser && x.IdHotel == idHotel)
                            .Include(x=>x.OrderDetails).ThenInclude(x=>x.IdRoomNavigation).OrderByDescending(x=>x.DateCreate);
                if (order != null)
                {
                    var room = order.First().OrderDetails.OrderByDescending(x=>x.DateCreate).FirstOrDefault();
                    var rating = _hotelRatingRepository.FindAll(x => x.IdUser == idUser && x.IdHotel == idHotel).SingleOrDefault();
                    var user = _userRepository.GetById(idUser);
                    userBoxReview.UserId = idUser;
                    userBoxReview.UserName = user.UserName;
                    userBoxReview.Avatar = user.Picture;
                    userBoxReview.RoomName = room.IdRoomNavigation.Name;
                    userBoxReview.NumMature = room.IdRoomNavigation.NumMature;
                    userBoxReview.NumChild = room.IdRoomNavigation.NumChild;
                    userBoxReview.DateReviews = rating.DateReviews;
                    userBoxReview.DateUpdate = rating.DateUpdate;
                    userBoxReview.Description = rating.Description;
                    userBoxReview.LocationScore = rating.LocationScore;
                    userBoxReview.ValueScore = rating.ValueScore;
                    userBoxReview.ServiceScore = rating.ServiceScore;
                    userBoxReview.CleanlinessScore = rating.CleanlinessScore;
                    userBoxReview.RoomsScore = rating.RoomsScore;
                    userBoxReview.CheckInDate = (DateTime)room.ChechIn;
                    userBoxReview.CheckOutDate = (DateTime)room.CheckOut;
                    response.Data = userBoxReview;
                    return response;
                }
                response.Message = $"{ErrorCodeMessage.NotFound.Value}";
                response.Data = userBoxReview;
                return response;

            }
            catch
            {
                response.Message = $"{ErrorCodeMessage.Exception.Value}";
                response.Data = userBoxReview;
                return response;
            }
        }

        private (UpdateRatingResponse,bool) UpdateScoreForHotel(RatingOrUpdateDto dto)
        {
            var exitsRating = _hotelRatingRepository.FindAll(x => x.IdUser != dto.UserId && x.IdHotel == dto.HotelId).ToList();
            var hotel = _hotelRepository.GetById(dto.HotelId);
            if (hotel == null)
                return (null,false);
            var count = exitsRating.Count() + 1;
            UpdateLocationScore(hotel, count, exitsRating, dto);
            UpdateValueScore(hotel, count, exitsRating, dto);
            UpdateServiceScore(hotel, count, exitsRating, dto);
            UpdateCleanlinessScore(hotel, count, exitsRating, dto);
            UpdateCleanRoomsScore(hotel, count, exitsRating, dto);
            UpdateReview_score(hotel);
            UpdateRatingResponse re = new UpdateRatingResponse();
            re.LocationScore = hotel.LocationScore;
            re.ValueScore = hotel.ValueScore;
            re.ServiceScore = hotel.ServiceScore;
            re.CleanlinessScore = hotel.CleanlinessScore;
            re.RoomsScore = hotel.RoomsScore;
            re.ReviewScore = hotel.ReviewScore;

            return (re,true);
        }

        private void UpdateLocationScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.LocationScore) + dto.LocationScore;
            hotel.LocationScore = totalScore / count;
        }
        private void UpdateValueScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.ValueScore) + dto.ValueScore;
            hotel.ValueScore = totalScore / count;
        }
        private void UpdateServiceScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.ServiceScore) + dto.ServiceScore;
            hotel.ServiceScore = totalScore / count;
        }
        private void UpdateCleanlinessScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.CleanlinessScore) + dto.CleanlinessScore;
            hotel.CleanlinessScore = totalScore / count;
        }
        private void UpdateCleanRoomsScore(Hotel hotel, int count, List<HotelRating> hotelRatings, RatingOrUpdateDto dto)
        {
            var totalScore = hotelRatings.Sum(x => x.RoomsScore) + dto.RoomsScore;
            hotel.RoomsScore = totalScore / count;
        }
        private void UpdateReview_score(Hotel hotel)
        {
            var totalScore = hotel.LocationScore + hotel.ValueScore + hotel.ServiceScore + hotel.CleanlinessScore + hotel.RoomsScore;
            hotel.ReviewScore = totalScore / 5;
        }
    }
}
