namespace Bangla.Services.OrderAPI.Utility
{
    public class OrderUtility
    {
        public enum Status
        {
            Pending,
            Approved,
            ReadyForPickup,
            Completed,
            Refunded,
            Cancelled
        }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
    }
}
