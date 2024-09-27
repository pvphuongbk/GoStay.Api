using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace GoStay.Common.Configuration
{
    public class AppConfigs
    {
        private static IConfiguration _configuration;
        public static void LoadAll(IConfiguration configuration)
        {
            _configuration = configuration;
            ApiUrlBase = GetConfigValue("Appconfig:ApiUrlBase", "");
            ApiKey = GetConfigValue("AppSettings:ApiKey", "");
            ApiAir = GetConfigValue("AppSettings:ApiAir", "");
            ItemPerPage = GetConfigValue("Appconfig:ItemPerPage", 50);
            SqlConnection = GetConfigValue("ConnectionStrings:GoStaySqlConn", "No connection");
            GoogleClientId = GetConfigValue("Google:ClientId", "No value");
            GoogleClientSecret = GetConfigValue("Google:ClientSecret", "No value");
            FacebookClientId = GetConfigValue("Facebook:ClientId", "No value");
            FacebookClientSecret = GetConfigValue("Facebook:ClientSecret", "No value");
            FullPath = GetConfigValue("Template:FullPath", "wwwroot\\upload\\");
            RootPath = GetConfigValue("Template:RootPath", "");
            GeneralPath = GetConfigValue("Template:GeneralPath", "");
        }
        public static string FormatCurrency(string currencyCode, decimal amount)
        {
            CultureInfo culture = (from c in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                   let r = new RegionInfo(c.LCID)
                                   where r != null
                                   && r.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper()
                                   select c).FirstOrDefault();
            if (culture == null)
            {
                // fall back to current culture if none is found
                // you could throw an exception here if that's not supposed to happen
                culture = CultureInfo.CurrentCulture;

            }
            culture = (CultureInfo)culture.Clone();
            culture.NumberFormat.CurrencySymbol = currencyCode;
            culture.NumberFormat.CurrencyPositivePattern = culture.NumberFormat.CurrencyPositivePattern == 0 ? 2 : 3;
            var cnp = culture.NumberFormat.CurrencyNegativePattern;
            switch (cnp)
            {
                case 0: cnp = 14; break;
                case 1: cnp = 9; break;
                case 2: cnp = 12; break;
                case 3: cnp = 11; break;
                case 4: cnp = 15; break;
                case 5: cnp = 8; break;
                case 6: cnp = 13; break;
                case 7: cnp = 10; break;
            }
            culture.NumberFormat.CurrencyNegativePattern = cnp;

            return amount.ToString("C" + ((amount % 1) == 0 ? "0" : "2"), culture);
        }
        const string BuildVersionMetadataPrefix = "+build";
        const string dateFormat = "yyyy-MM-ddTHH:mm:ss:fffZ";
        public static int IdDomain = 1;
        public static string ApiKey { get; set; }
        public static string FullPath { get; set; }
        public static string RootPath { get; set; }
        public static string GeneralPath { get; set; }

        public static List<int> AdminIds { get; set; } = new List<int>() { 9 };
        public static DateTime GetLinkerTime(Assembly assembly)
        {
            var attribute = assembly
              .GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value[(index + BuildVersionMetadataPrefix.Length)..];

                    return DateTime.ParseExact(
                        value,
                      dateFormat,
                      CultureInfo.InvariantCulture);
                }
            }
            return default;
        }
        public static string buidinfo()
        {
            return "Gostay 3.0 - latest build: " + GetLinkerTime(Assembly.GetEntryAssembly()) + " (beta version).";
        }
        public static string checkinDate = DateTime.Today.ToString("dd/MM/yyyy");
        public static string checkoutDate = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
        //public static string OrderCode = DateTime.UtcNow.TimeOfDay.TotalSeconds.ToString();
        public static DateTime startDate = new DateTime(2022, 1, 1);
        public static string ApiUrlBase { get; set; }
        public static string ApiAir { get; set; }

        public static string GoogleClientId { get; set; }
        public static string GoogleClientSecret { get; set; }
        public static string FacebookClientId { get; set; }
        public static string FacebookClientSecret { get; set; }
        public static int ItemPerPage { get; set; }
        public static string SqlConnection { get; set; }
        public static string CurrentUserCK = "CurrentUser";
        public static string CTNAME = "Việt Nam";
        public static string strTitle = "Gostay - Nền tảng thương mại điện tử cho du lịch hàng đầu Việt Nam";
        /// <summary>
        /// plhd
        /// </summary>
        /// 

        public static string pls(string _str)
        {
            return "Bạn cần nhập vào " + _str;
        }

        public static int acvivemenu = 0;
        /// <summary>
        /// Lấy ra giá trị config trong file .config
        /// </summary>
        private static T GetConfigValue<T>(string configKey, T defaultValue)
        {
            var value = defaultValue;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                if (converter != null)
                {
                    var setting = _configuration.GetSection(configKey).Value;
                    if (!string.IsNullOrEmpty(setting))
                    {
                        value = (T)converter.ConvertFromString(setting);
                    }
                }
            }
            catch
            {
                value = defaultValue;
            }
            return value;
        }

        public static string Activecss(int tab, int tabactive)
        {
            if (tab == tabactive)
            {
                return "active";
            }
            else
            {
                return "";
            }
        }
    }
}
