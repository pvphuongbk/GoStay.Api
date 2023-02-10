namespace GoStay.DataDto.RatingDto
{
    public class RatingOrUpdateDto
    {
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public string Description { get; set; }
        public decimal LocationScore { get; set; }
        public decimal ValueScore { get; set; }
        public decimal ServiceScore { get; set; }
        public decimal CleanlinessScore { get; set; }
        public decimal RoomsScore { get; set; }
    }
}
