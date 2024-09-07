namespace Future.Bangla.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; } // we need the url of the API
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
