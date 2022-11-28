using System.ComponentModel;

namespace GoStay.Common.Extention
{
	public static class StringExtensions
	{
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
	}
}
