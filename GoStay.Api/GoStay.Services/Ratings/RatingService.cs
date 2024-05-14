using GoStay.Common.Extention;
using GoStay.Common.Helpers;
using GoStay.Data.Base;
using GoStay.DataAccess.Entities;
using GoStay.DataAccess.Interface;
using GoStay.DataAccess.UnitOfWork;
using GoStay.DataDto.RatingDto;
using GoStay.Repository.Repositories;
using GoStay.Services.Ratings;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static System.Formats.Asn1.AsnWriter;

namespace GoStay.Services.Reviews
{
    public class RatingService : IRatingService
    {
        private readonly ICommonRepository<HotelRating> _hotelRatingRepository;
        private readonly ICommonUoW _icommonUoWRepository;
        private readonly ICommonRepository<Hotel> _hotelRepository;
        private readonly ICommonRepository<Order> _orderRepository;
        private readonly ICommonRepository<User> _userRepository;
        private readonly ICommonUoW _commonUoW;
        private readonly ICommonRepository<OrderDetail> _orderDetailRepository;

        public RatingService(ICommonRepository<HotelRating> hotelRatingRepository, ICommonUoW icommonUoWRepository,
            ICommonRepository<Hotel> hotelRepository, ICommonRepository<Order> orderRepository
            , ICommonRepository<OrderDetail> orderDetailRepository, ICommonRepository<User> userRepository,ICommonUoW commonUoW)
        {
            _hotelRatingRepository = hotelRatingRepository;
            _icommonUoWRepository = icommonUoWRepository;
            _hotelRepository = hotelRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _userRepository = userRepository;
            _commonUoW = commonUoW;
        }
        
        public ResponseBase GetRatingByUser(int hotelId, int userId)
        {
            ResponseBase responseBase = new ResponseBase();
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

                    responseBase.Data = dto;
                }

                return responseBase;
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetRatingByHotel(int hotelId)
        {
            ResponseBase responseBase = new ResponseBase();
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
                responseBase.Data = res;
                return responseBase;
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetListRating(int? HotelId, byte? Status,string? NameSearch, int PageIndex, int PageSize)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                if (NameSearch == null)
                {
                    NameSearch = "";
                }
                NameSearch = NameSearch.RemoveUnicode();
                NameSearch = NameSearch.Replace(" ", string.Empty).ToLower();

                var Ratings = HotelRepository.GetListRating(HotelId, Status, NameSearch, PageIndex, PageSize);

                responseBase.Code = ErrorCodeMessage.Success.Key;
                responseBase.Message = ErrorCodeMessage.Success.Value;
                responseBase.Data = Ratings;
                return responseBase;
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase ReviewOrUpdateScore(RatingOrUpdateDto dto)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                dto.LocationScore = CheckScore(dto.LocationScore);
                dto.RoomsScore = CheckScore(dto.RoomsScore);
                dto.CleanlinessScore = CheckScore(dto.CleanlinessScore);
                dto.ValueScore = CheckScore(dto.ValueScore);
                dto.ServiceScore = CheckScore(dto.ServiceScore);

                _icommonUoWRepository.BeginTransaction();
                var (re, check) = UpdateScoreForHotel(dto);
                if (!check)
                {
                    responseBase.Message = "User Id không tồn tại";
                    responseBase.Code = ErrorCodeMessage.NotFound.Key;
                    return responseBase;
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
                        Status = 0,
                    };
                    _hotelRatingRepository.Insert(item);
                }

                _icommonUoWRepository.Commit();
                responseBase.Data = re;
                return responseBase;
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase UpdateStatusRating(int Id, byte status)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var exitsRating = _hotelRatingRepository.FindAll(x => x.Id ==Id).SingleOrDefault();
                if(exitsRating == null)
                {
                    responseBase.Code = ErrorCodeMessage.NotFound.Key;
                    responseBase.Message = ErrorCodeMessage.NotFound.Value;
                    return responseBase;
                }
                exitsRating.Status = status;
                _hotelRatingRepository.Update(exitsRating);

                _icommonUoWRepository.Commit();
                responseBase.Data = "Success";
                return responseBase;
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }
        public ResponseBase GetUserBoxReview(int idHotel)
        {
            ResponseBase responseBase = new ResponseBase();
            List<UserBoxReview> listUserBoxReview = new List<UserBoxReview>();
            var rating = _hotelRatingRepository.FindAll(x=>x.IdHotel == idHotel && x.Status==1);

            try
            {
                
                if (rating != null)
                {
                    foreach (var item in rating)
                    {
                        var user = CreatUserBoxReview(item);
                        listUserBoxReview.Add(user);
                    }
                    responseBase.Data = listUserBoxReview;
                    return responseBase;
                }
                responseBase.Message = $"{ErrorCodeMessage.NotFound.Value}";
                responseBase.Data = listUserBoxReview;
                return responseBase;

            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
            }
        }

        public ResponseBase CheckOrdered(int hotelId, int userId)
        {
            ResponseBase responseBase = new ResponseBase();
            try
            {
                var exist = _orderRepository.FindAll(x => x.IdUser == userId && x.IdHotel == hotelId).Count();
                if (exist > 0)
                {
                    responseBase.Data = 1;
                    return responseBase;
                }
                else
                {
                    responseBase.Data = 0;
                    return responseBase;
                }
            }
            catch (Exception e)
            {
                FileHelper.GeneratorFileByDay(Common.Enums.FileStype.Error, e.ToString(), "Rating");
                responseBase.Message = e.Message;
                return responseBase;
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

        public decimal CheckScore(decimal score)
        {
            if (score > 10)
                return score / 10;
            return score;
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
        private UserBoxReview CreatUserBoxReview(HotelRating rating)
        {
            UserBoxReview userBoxReview = new UserBoxReview();
            var order = _orderRepository.FindAll(x => x.IdHotel == rating.IdHotel&& x.IdUser == rating.IdUser)
                            .Include(x => x.OrderDetails).ThenInclude(x => x.IdRoomNavigation)
                            .Include(x => x.IdUserNavigation).OrderByDescending(x=>x.DateCreate).FirstOrDefault();

            //var rating = _hotelRatingRepository.FindAll(x => x.IdUser == order.IdUser && x.IdHotel == order.IdHotel).SingleOrDefault();

            var user = _userRepository.GetById(rating.IdUser);
            userBoxReview.UserId = rating.IdUser;
            userBoxReview.UserName = user.UserName;
            userBoxReview.Avatar = user.Picture;
            userBoxReview.DateReviews = rating.DateReviews;
            userBoxReview.DateUpdate = rating.DateUpdate;
            userBoxReview.Description = rating.Description;
            userBoxReview.LocationScore = rating.LocationScore;
            userBoxReview.ValueScore = rating.ValueScore;
            userBoxReview.ServiceScore = rating.ServiceScore;
            userBoxReview.CleanlinessScore = rating.CleanlinessScore;
            userBoxReview.RoomsScore = rating.RoomsScore;
            userBoxReview.CheckInDate = null;
            userBoxReview.CheckOutDate = null;
            if (order != null)
            {

                    var room = order.OrderDetails.OrderByDescending(x => x.DateCreate).First();

                    userBoxReview.RoomName = room.IdRoomNavigation.Name;
                    userBoxReview.NumMature = room.IdRoomNavigation.NumMature;
                    userBoxReview.NumChild = room.IdRoomNavigation.NumChild;
                    userBoxReview.CheckInDate = (DateTime)room.ChechIn;
                    userBoxReview.CheckOutDate = (DateTime)room.CheckOut;

                return userBoxReview;
            }
            return userBoxReview;
        }

    }
}
