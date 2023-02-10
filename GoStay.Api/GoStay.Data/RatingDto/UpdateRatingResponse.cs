namespace GoStay.DataDto.RatingDto
{
    public class UpdateRatingResponse
    {
        public string Description { get; set; }
        public decimal? LocationScore { get; set; }
        public decimal? ValueScore { get; set; }
        public decimal? ServiceScore { get; set; }
        public decimal? CleanlinessScore { get; set; }
        public decimal? RoomsScore { get; set; }
        public decimal? ReviewScore { get; set; }
    }
}
