namespace GoStay.DataDto.RatingDto
{
    public class GetRatingByHotelDto
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Description { get; set; }
        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
    }
}
