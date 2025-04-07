using System.ComponentModel;

using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GoStay.Common.Extention
{
	public static class StringExtensions
	{
        public static string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                                            "đ",
                                            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                                            "í","ì","ỉ","ĩ","ị",
                                            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                                            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                                            "ý","ỳ","ỷ","ỹ","ỵ",};
        public static string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                                            "d",
                                            "e","e","e","e","e","e","e","e","e","e","e",
                                            "i","i","i","i","i",
                                            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                                            "u","u","u","u","u","u","u","u","u","u","u",
                                            "y","y","y","y","y",};

        public static string[] arr3 = new string[] {",",".","+","-","_","(",")","*","&","^","%","$","#","@","!","`","~","{","}","[","]",
                                                            ":",";","'","\"","\\","|",">","<","/","?"};

        public static string FirstCharToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
        }
        public static string GetEnumDescription(this System.Enum enumValue)
		{
			var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

			var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
		}

        public static string RemoveUnicode(this string text)
        {

            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
        public static string RemoveUnicode2(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string normalizedString = text.Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();
            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string ReplaceSpecialChar(this string text)
        {
            foreach (var c in arr3)
            {
                if(text.Contains(c))
                {
                    text=text.Replace(c,string.Empty);
                }    
            }
            text=text.Replace(" ", "-");
            return text;
        }
    }
}
