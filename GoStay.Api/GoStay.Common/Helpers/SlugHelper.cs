using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace GoStay.Web.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return "";

            title = title.ToLowerInvariant().Trim();

            // Chuyển đổi tiếng Việt có dấu thành không dấu
            title = RemoveDiacritics(title);

            // Thay khoảng trắng và ký tự đặc biệt bằng dấu '-'
            title = Regex.Replace(title, @"[^a-z0-9\s-]", ""); // Loại bỏ ký tự đặc biệt
            title = Regex.Replace(title, @"\s+", "-").Trim('-'); // Thay khoảng trắng bằng '-'

            return title;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
    public static class VietnameseNormalizer
    {
        public static string NormalizeVietnamese(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Chuyển đổi từng ký tự giữ nguyên chữ hoa/chữ thường
            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(NormalizeChar(c));
                }
            }

            return stringBuilder.ToString();
        }

        private static char NormalizeChar(char c)
        {
            // Bảng chuyển đổi giữ nguyên chữ hoa/chữ thường
            switch (c)
            {
                case 'À': case 'Á': case 'Ả': case 'Ã': case 'Ạ': case 'Ă': case 'Ằ': case 'Ắ': case 'Ẳ': case 'Ẵ': case 'Ặ': case 'Â': case 'Ầ': case 'Ấ': case 'Ẩ': case 'Ẫ': case 'Ậ': return 'A';
                case 'à': case 'á': case 'ả': case 'ã': case 'ạ': case 'ă': case 'ằ': case 'ắ': case 'ẳ': case 'ẵ': case 'ặ': case 'â': case 'ầ': case 'ấ': case 'ẩ': case 'ẫ': case 'ậ': return 'a';

                case 'È': case 'É': case 'Ẻ': case 'Ẽ': case 'Ẹ': case 'Ê': case 'Ề': case 'Ế': case 'Ể': case 'Ễ': case 'Ệ': return 'E';
                case 'è': case 'é': case 'ẻ': case 'ẽ': case 'ẹ': case 'ê': case 'ề': case 'ế': case 'ể': case 'ễ': case 'ệ': return 'e';

                case 'Ì': case 'Í': case 'Ỉ': case 'Ĩ': case 'Ị': return 'I';
                case 'ì': case 'í': case 'ỉ': case 'ĩ': case 'ị': return 'i';

                case 'Ò': case 'Ó': case 'Ỏ': case 'Õ': case 'Ọ': case 'Ô': case 'Ồ': case 'Ố': case 'Ổ': case 'Ỗ': case 'Ộ': case 'Ơ': case 'Ờ': case 'Ớ': case 'Ở': case 'Ỡ': case 'Ợ': return 'O';
                case 'ò': case 'ó': case 'ỏ': case 'õ': case 'ọ': case 'ô': case 'ồ': case 'ố': case 'ổ': case 'ỗ': case 'ộ': case 'ơ': case 'ờ': case 'ớ': case 'ở': case 'ỡ': case 'ợ': return 'o';

                case 'Ù': case 'Ú': case 'Ủ': case 'Ũ': case 'Ụ': case 'Ư': case 'Ừ': case 'Ứ': case 'Ử': case 'Ữ': case 'Ự': return 'U';
                case 'ù': case 'ú': case 'ủ': case 'ũ': case 'ụ': case 'ư': case 'ừ': case 'ứ': case 'ử': case 'ữ': case 'ự': return 'u';

                case 'Ỳ': case 'Ý': case 'Ỷ': case 'Ỹ': case 'Ỵ': return 'Y';
                case 'ỳ': case 'ý': case 'ỷ': case 'ỹ': case 'ỵ': return 'y';

                case 'Đ': return 'D';
                case 'đ': return 'd';

                default: return c;
            }
        }
    }
}
