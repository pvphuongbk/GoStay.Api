

namespace GoStay.DataDto.News
{
    public partial class NewsDto
    {
        public int? Id { get; set; }
        public int? IdCategory { get; set; }
        public int? IdUser { get; set; }

        public string? Keysearch { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? PictureTitle { get; set; }
        public int? LangId { get; set; }
        public int? IdTopic { get; set; }
    }
    public partial class NewsDetailDto
    {
        public int? Id { get; set; }
        public int? IdCategory { get; set; }
        public int? IdUser { get; set; }
        public byte? Status { get; set; }
        public string? Title { get; set; } = null!;
        public string? Content { get; set; }
        public string Category { get; set; }
        public string? PictureTitle { get; set; }
        public string? Description { get; set; }
        public int? LangId { get; set; }
        public int? IdTopic { get; set; }
        public string? UserName { get; set; }
        public DateTime DateCreate { get; set; }
    }
    public class GetListNewsParam
    {
        public int? UserId { get; set; }
        public int? IdCategory { get; set; }
        public int? IdTopic { get; set; }

        public byte? Status { get; set; }
        public string? TextSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    public class NewSearchOutDto
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreate { get; set; }
        public int IdCategory { get; set; }

        public string? PictureTitle { get; set; }
        public string? Description { get; set; }

        public string Category { get; set; }
        public string UserName { get; set; }
        public int Total { get; set; }
        public int TotalPicture { get; set; }

        private string Urls
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    Pictures = new List<string>();
                else
                {
                    Pictures = value.Split(';').ToList();

                }
            }
        }
        public List<string> Pictures { get; set; } = new List<string>();
        public string Topic { get; set; }
        public int IdTopic { get; set; }

        public string Language { get; set; }
        public int LangId { get; set; }

    }
    public class EditNewsContentParam
    {
        public string Content { get; set; }
        public int NewsId { get; set; }
    }
    public class EditNewsPictureTitleParam
    {
        public string Url { get; set; }
        public int NewsId { get; set; }
    }
}
