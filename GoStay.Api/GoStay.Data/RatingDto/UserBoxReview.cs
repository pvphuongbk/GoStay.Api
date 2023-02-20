using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.RatingDto
{
    public class UserBoxReview
    {
        //user
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        //room
        public string RoomName { get; set; }
        public byte? NumMature { get; set; }
        public byte? NumChild { get; set; }
        //rating
        public DateTime? DateReviews { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string Description { get; set; }
        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
        //orderdetail
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        
    }
}
