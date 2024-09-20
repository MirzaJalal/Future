namespace Future.Bangla.Web.Utility
{
    public class SD
    {
        public static string CouponAPIBase { get; set; } // we need the url of the API
        public static string AuthenticationAPIBase { get; set; } // we need the url of the API

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JwtTokenClaimVerification";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
