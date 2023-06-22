namespace GoStay.Data.Base
{
	public class ErrorCodeMessage
	{
		#region Return Code (0 - 99): Common error
		public static readonly KeyValuePair<int, string> Success = new KeyValuePair<int, string>(0, "The operation completed successfully.");
		public static readonly KeyValuePair<int, string> Exception = new KeyValuePair<int, string>(1, "An error occurred.");
        public static readonly KeyValuePair<int, string> NotFound = new KeyValuePair<int, string>(2, "Not Found.");
        public static readonly KeyValuePair<int, string> NoPermission = new KeyValuePair<int, string>(3, "No Permission");

        #endregion
    }

    public class CheckOrderCodeMessage
    {
        #region Return Code (0 - 99): Common error
        public static readonly KeyValuePair<int, string> CreateNewOrder = new KeyValuePair<int, string>(0, "Create new order");
        public static readonly KeyValuePair<int, string> GetOldOrder = new KeyValuePair<int, string>(2, "Get Old Order");
        public static readonly KeyValuePair<int, string> UpdateOrder = new KeyValuePair<int, string>(3, "Update Order");

        #endregion
    }
}
